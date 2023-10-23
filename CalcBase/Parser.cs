using System;
using System.Runtime.CompilerServices;
using System.Text;
using CalcBase.Functions;
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
            new ModulusOperator(),
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
            new SinFunction()
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
            StringBuilder text = new();

            for (int i = start; i < infix.Length; i++)
            {
                char c = infix[i];
                if (char.IsLetterOrDigit(c))
                {
                    text.Append(c);
                }
                else
                {
                    break;
                }
            }

            // Return token and end position
            return new TextToken()
            {
                Position = start,
                Length = text.Length,
                Text = text.ToString()
            };
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
                    throw new ExpressionException("Syntax error", i, 1);
                }
            }

            return tokens;
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
                if (token is IntegerNumberToken)
                {
                    postfix.Add(token);
                }
                else if (token is RealNumberToken)
                {
                    postfix.Add(token);
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
                else if (token is OperatorToken opToken)
                {                    
                    while (opStack.TryPeek(out IToken? stackedToken) &&
                        (stackedToken is OperatorToken stackedOpToken) &&
                        (opToken.Operator.Precedence > stackedOpToken.Operator.Precedence))
                    {
                        //System.Diagnostics.Debug.WriteLine($"  Op to queue {stackedOpToken}");
                        postfix.Add(opStack.Pop());                    
                    }

                    opStack.Push(opToken);
                }
                else if (token is FunctionToken funcToken)
                { 
                    // TODO
                }
                else
                {
                    throw new NotImplementedException($"Unhandled token: {token}");
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
