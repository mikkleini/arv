using System;
using System.Runtime.CompilerServices;
using System.Text;
using CalcBase.Constants;
using CalcBase.Constants.Mathematical;
using CalcBase.Functions;
using CalcBase.Generic;
using CalcBase.Operators;
using CalcBase.Operators.Arithmetic;
using CalcBase.Operators.Bitwise;
using CalcBase.Tokens;

[assembly: InternalsVisibleTo("CalcBaseTest")]
namespace CalcBase
{
    /// <summary>
    /// Expression parser
    /// </summary>
    public partial class Parser
    {
        private static readonly string BinaryNumberPrefix = "0b";
        private static readonly string HexadecimalNumberPrefix = "0x";
        private static readonly char RadixPointSymbol = '.';
        private static readonly char ArgumentSeparatorSymbol = ',';
        private static readonly char ExponentPointSymbol = 'E';
        private static readonly int IntTypeBits = 128;

        // Operators
        private static readonly IOperator[] operators = new IOperator[]
        {
            new AdditionOperator(),
            new SubtractionOperator(),
            new MultiplicationOperator(),
            new DivisionOperator(),
            new ReminderOperator(),
            new ExponentOperator(),
            new QuotientOperator(),
            new NegationOperator(),
            new AndOperator(),
            new OrOperator(),
            new XorOperator(),
            new InverseOperator(),
        };

        private static readonly IFunction[] functions = new IFunction[]
        {
            new CosFunction(),
            new SinFunction(),
            new RoundFunction(),
        };

        private static readonly IConstant[] constants = new IConstant[]
        {
            new PiConstant(),
        };

        /// <summary>
        /// Read text (don't yet know if it's constant, unit, variable or function)
        /// </summary>
        /// <param name="infix">Infix expression</param>
        /// <param name="start">Number start position</param>
        /// <returns>Text token</returns>
        /// <exception cref="ExpressionException">Error in expression</exception>
        internal static IToken ReadText(ReadOnlySpan<char> infix, int start)
        {
            int pos;

            for (pos = start; pos < infix.Length; pos++)
            {
                char c = infix[pos];
                if (!char.IsLetterOrDigit(c))
                {
                    break;
                }
            }

            string text = infix.Slice(start, pos - start).ToString();

            // Is it a function ?
            IFunction? func = functions.FirstOrDefault(o =>
                string.Equals(o.Symbol, text, StringComparison.InvariantCultureIgnoreCase));
            if (func != null)
            {
                return new FunctionToken()
                {
                    Position = start,
                    Length = text.Length,
                    Function = func
                };
            }

            // Is it a constant ?
            IConstant? cnst = constants.FirstOrDefault(o =>
                string.Equals(o.Symbol, text, StringComparison.InvariantCultureIgnoreCase) ||
                string.Equals(o.SimpleSymbol, text, StringComparison.InvariantCultureIgnoreCase));
            if (cnst != null)
            {
                return new ConstantToken()
                {
                    Position = start,
                    Length = text.Length,
                    Constant = cnst
                };
            }

            throw new ExpressionException($"Unknown function/constant: {text}", start, text.Length);
        }

        /// <summary>
        /// Minus operator requires special handling
        /// TODO Maybe there's more universal way to resolve conflicting symbols?
        /// </summary>
        /// <param name="prevToken">Previous token or null if none</param>
        /// <returns>Operator</returns>
        internal static IOperator DecideMinusOperator(IToken? prevToken)
        {
            bool isNegation = false;

            if (prevToken == null)
            {
                isNegation = true;
            }
            else
            {
                if (prevToken is LeftParenthesisToken)
                {
                    isNegation = true;
                }
                else if (prevToken is OperatorToken)
                {
                    isNegation = true;
                }
            }

            // So which operator it is
            if (isNegation)
            {
                return operators.Single(op => op is NegationOperator);
            }
            else
            {
                return operators.Single(op => op is SubtractionOperator);
            }
        }

        /// <summary>
        /// Tokenize infix expression
        /// </summary>
        /// <returns></returns>
        public static List<IToken> Tokenize(string infix)
        {
            List<IToken> tokens = new();
            int i = 0;

            while (i < infix.Length)
            {
                char c = infix[i];
                IToken? token = null;

                if (char.IsWhiteSpace(c))
                {
                    // Ignore
                    i++;
                    continue;
                }
                else if (char.IsDigit(c))
                {
                    token = ReadNumber(infix, i);
                }
                else if (char.IsLetter(c))
                {
                    token = ReadText(infix, i);
                }
                else if (c == '(')
                {
                    token = new LeftParenthesisToken()
                    {
                        Position = i,
                        Length = 1
                    };
                }
                else if (c == ')')
                {
                    token = new RightParenthesisToken()
                    {
                        Position = i,
                        Length = 1
                    };
                }
                else if (c == ArgumentSeparatorSymbol)
                {
                    token = new ArgumentSeparatorToken()
                    {
                        Position = i,
                        Length = 1
                    };
                }
                else
                {
                    IOperator? op = null;

                    // Special case with minus operator
                    if (c == '-')
                    {
                        op = DecideMinusOperator(tokens.LastOrDefault());
                    }
                    else
                    {
                        // Check for operators by length of the symbol.
                        // Basically if expression contains ** then it should first try to find exponent operator **, not the multiplication.
                        op = operators
                            .OrderByDescending(o => o.Symbol.Length)
                            .FirstOrDefault(o => infix.AsSpan().Slice(i).StartsWith(o.Symbol.AsSpan()));
                    }

                    // It's an operator ?
                    if (op != null)
                    {
                        token = new OperatorToken()
                        {
                            Position = i,
                            Length = op.Symbol.Length,
                            Operator = op
                        };
                    }
                }

                // Found token ?
                if (token != null)
                {
                    tokens.Add(token);
                    i += token.Length;
                }
                else
                {
                    throw new ExpressionException($"Syntax error ({c} at {i})", i, 1);
                }
            }

            return tokens;
        }

        /// <summary>
        /// Count and check function arguments
        /// </summary>
        /// <param name="infix">Infix tokens</param>
        /// <param name="funcIndex">Function index</param>
        /// <returns></returns>
        private static void CheckFunctionArguments(List<IToken> infix, int funcIndex)
        {
            FunctionToken funcToken = (FunctionToken)infix[funcIndex];
            int numExpectedArgs = funcToken.ArgumentCount;
            int numActualArgs = 0;
            int funcParenthesisDepth = 0;

            // Count arguments
            for (int i = funcIndex + 2; i < infix.Count; i++)
            {
                IToken? previous = (i > 0 ? infix[i - 1] : null);
                IToken? current = infix[i];

                if (current is LeftParenthesisToken)
                {
                    funcParenthesisDepth++;
                }
                else if (current is RightParenthesisToken)
                {
                    funcParenthesisDepth--;
                    if (funcParenthesisDepth < 0)
                    {
                        // It's the end of the function
                        numActualArgs++;
                        break;
                    }
                }
                else if (current is ArgumentSeparatorToken)
                {
                    if (previous is ArgumentSeparatorToken)
                    {
                        throw new ExpressionException("Missing argument", current);
                    }

                    numActualArgs++;
                }
            }

            if (numActualArgs < numExpectedArgs)
            {
                throw new ExpressionException($"Missing function argument(s)", funcToken);
            }
            else if (numActualArgs > numExpectedArgs)
            {
                throw new ExpressionException($"Excessive function argument(s)", funcToken);
            }
        }

        /// <summary>
        /// Check for infix errors
        /// </summary>
        /// <param name="infix">Infix tokens</param>
        /// <exception cref="ExpressionException"></exception>
        public static void InfixErrorCheck(List<IToken> infix, int startIndex = 0)
        {
            int parenthesisDepth = 0;

            if (infix.Count == 0)
            {
                throw new ExpressionException("Empty expression", 0, 0);
            }

            // Check token by token
            for (int i = 0; i < infix.Count; i++)
            {
                IToken? previous = (i > 0 ? infix[i - 1] : null);
                IToken? current = infix[i];
                IToken? next = (i < infix.Count - 1 ? infix[i + 1] : null);

                if (current is LeftParenthesisToken)
                {
                    parenthesisDepth++;
                }
                else if (current is RightParenthesisToken)
                {
                    parenthesisDepth--;
                    if (parenthesisDepth < 0)
                    {
                        throw new ExpressionException("Excessive closing parenthesis", current);
                    }
                }

                // Empty parenthesis not used with function
                if ((previous is not FunctionToken) && (current is LeftParenthesisToken) && (next is RightParenthesisToken))
                {
                    throw new ExpressionException("Empty parenthesis", current.Position, next.Position - current.Position + 1);
                }

                // Make sure function is followed by opening parenthesis
                if ((current is FunctionToken) && (next is not LeftParenthesisToken))
                {
                    throw new ExpressionException("Missing parenthesis for argument(s)", current);
                }

                // Check function argument count (must be after parenthesis check)
                if (current is FunctionToken)
                {
                    CheckFunctionArguments(infix, i);
                }
            }

            // Any missing closing parenthesis ?
            if (parenthesisDepth > 0)
            {
                throw new ExpressionException($"Missing closing parenthesis", infix.Last().Position + 1, 1);
            }
        }

        /// <summary>
        /// Shunting yard algoritm to get postfix (RPN) expression from infix tokens
        /// </summary>
        /// <param name="infix">Token in infix order</param>
        /// <returns>List of tokens in postfix (RPN) order</returns>
        public static List<IToken> ShuntingYard(List<IToken> infix)
        {
            List<IToken> postfix = new();
            Stack<IToken> opStack = new();

            foreach (IToken token in infix)
            {
                if (token is NumberToken)
                {
                    postfix.Add(token);
                }
                else if (token is ConstantToken constToken)
                {
                    postfix.Add(constToken);
                }
                else if (token is LeftParenthesisToken)
                {
                    opStack.Push(token);
                }
                else if (token is RightParenthesisToken)
                {
                    while (opStack.TryPeek(out IToken? stackedToken) && (stackedToken is OperatorToken stackedOpToken))
                    {
                        postfix.Add(opStack.Pop());
                    }
                    opStack.Pop();
                }
                else if (token is ArgumentSeparatorToken)
                {
                    while (opStack.TryPeek(out IToken? stackedToken) && (stackedToken is OperatorToken stackedOpToken))
                    {
                        postfix.Add(opStack.Pop());
                    }
                }
                else if (token is OperatorToken opToken)
                {                    
                    while (opStack.TryPeek(out IToken? stackedToken) &&
                        (((stackedToken is OperatorToken stackedOpToken) &&
                        (opToken.Operator.Precedence > stackedOpToken.Operator.Precedence)) ||
                        (stackedToken is FunctionToken)))
                    {
                        //System.Diagnostics.Debug.WriteLine($"  Op to queue {stackedOpToken}");
                        postfix.Add(opStack.Pop());                    
                    }

                    opStack.Push(opToken);
                }
                else if (token is FunctionToken funcToken)
                {
                    opStack.Push(funcToken);
                }
                else
                {
                    // Something's wrong
                    throw new ExpressionException($"Unhandled token: {token}", token.Position, token.Length);
                }
            }

            while (opStack.TryPop(out IToken? opToken))
            {
                postfix.Add(opToken);
            }

            return postfix;
        }
    }
}
