using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using CalcBase.Constants;
using CalcBase.Formulas;
using CalcBase.Functions;
using CalcBase.Numbers;
using CalcBase.Operators;
using CalcBase.Quantities;
using CalcBase.Tokens;
using CalcBase.Units;

[assembly: InternalsVisibleTo("CalcBaseTest")]
namespace CalcBase
{
    /// <summary>
    /// Expression parser
    /// </summary>
    public partial class Parser
    {
        public static readonly string BinaryNumberPrefix = "0b";
        public static readonly string HexadecimalNumberPrefix = "0x";
        public static readonly char RadixPointSymbol = '.';
        public static readonly char ArgumentSeparatorSymbol = ',';
        public static readonly char ExponentPointSymbolUpper = 'E';
        public static readonly char ExponentPointSymbolLower = 'e';

        /// <summary>
        /// Read text (don't yet know if it's constant, unit, variable or function)
        /// </summary>
        /// <param name="infix">Infix expression</param>
        /// <param name="start">Number start position</param>
        /// <returns>Text token</returns>
        /// <exception cref="ExpressionException">Error in expression</exception>
        internal static IToken ReadText(ReadOnlySpan<char> infix, int start)
        {
            string symbol;
            string text = infix.Slice(start).ToString();

            // Is it a function ?
            IFunction? func = Factory.Functions
                .OrderByDescending(f => f.Symbol.Length)
                .FirstOrDefault(f => text.StartsWith(f.Symbol));
            if (func != null)
            {
                return new FunctionToken()
                {
                    Position = start,
                    Length = func.Symbol.Length,
                    Function = func
                };
            }

            // Is it a constant ?
            (symbol, IConstant? constant) = Factory.ConstantsBySymbols
                .FirstOrDefault(c => text.StartsWith(c.symbol));
            if (constant != null)
            {
                return new ConstantToken()
                {
                    Position = start,
                    Length = symbol.Length,
                    Constant = constant
                };
            }

            // Is it a unit ?
            (symbol, IUnit? unit) = Factory.UnitsBySymbols
                .FirstOrDefault(u => text.StartsWith(u.symbol));
            if (unit != null)
            {
                return new UnitToken()
                {
                    Position = start,
                    Length = symbol.Length,
                    Unit = unit
                };
            }

            // Is it a SI unit multiple ?
            (symbol, ISIUnit? siUnit, UnitMultiple multiple) = Factory.SIUnitMultiplesBySymbols
                .FirstOrDefault(u => text.StartsWith(u.symbol));
            if (siUnit != null)
            {
                return new UnitToken()
                {
                    Position = start,
                    Length = symbol.Length,
                    Unit = siUnit,
                    UnitMultiple = multiple
                };
            }

            throw new ExpressionException($"Unknown function/constant/unit: {text}", start, text.Length);
        }

        /// <summary>
        /// Minus operator requires special handling
        /// TODO Maybe there's more universal way to resolve conflicting symbols?
        /// </summary>
        /// <param name="prevToken">Previous token or null if none</param>
        /// <returns>Operator</returns>
        internal static IOperator DecideMinusOperator(IToken? prevToken)
        {
            if (prevToken == null)
            {
                return Factory.Negation;
            }
            else if (prevToken is LeftParenthesisToken)
            {
                return Factory.Negation;
            }
            else if (prevToken is ArgumentSeparatorToken)
            {
                return Factory.Negation;
            }
            else if (prevToken is OperatorToken)
            {
                return Factory.Negation;
            }

            return Factory.Subtraction;
        }

        /// <summary>
        /// Tokenize infix expression
        /// </summary>
        /// <returns></returns>
        public static List<IToken> Tokenize(string infix)
        {
            List<IToken> tokens = [];
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
                        op = Factory.Operators
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
        /// Shunting yard algoritm to get postfix (RPN) expression tokens from infix tokens
        /// </summary>
        /// <param name="infix">Token in infix order</param>
        /// <returns>List of tokens in postfix (RPN) order</returns>
        public static List<IToken> ShuntingYard(List<IToken> infix)
        {
            List<IToken> postfix = [];
            Stack<IToken> opStack = new();

            for (int i = 0; i < infix.Count; i++)
            {
                IToken? current = infix[i];
                IToken? next = (i < infix.Count - 1 ? infix[i + 1] : null);
                
                if (current is NumberToken numberToken)
                {
                    if (next is UnitToken unitToken)
                    {
                        NumberType numberValue = numberToken.Number.Value;

                        // Is it multiple of SI unit ?
                        if ((unitToken.UnitMultiple != null) && (unitToken.Unit is ISIUnit))
                        {
                            numberValue *= unitToken.UnitMultiple.Value.Factor;
                        }

                        // Combine number and unit token into measure token
                        postfix.Add(new MeasureToken()
                        {
                            Position = numberToken.Position,
                            Length = unitToken.Position + numberToken.Length - numberToken.Position,
                            Measure = new Measure(numberToken.Number with { Value = numberValue }, unitToken.Unit)
                        });

                        // Leap over unit token
                        i++;
                    }
                    else
                    {
                        postfix.Add(numberToken);
                    }
                }
                else if (current is ConstantToken constToken)
                {
                    postfix.Add(constToken);
                }
                else if (current is LeftParenthesisToken leftParenthesisToken)
                {
                    opStack.Push(leftParenthesisToken);
                }
                else if (current is RightParenthesisToken)
                {
                    while (opStack.TryPeek(out IToken? stackedToken) && ((stackedToken is OperatorToken) || (stackedToken is FunctionToken)))
                    {
                        postfix.Add(opStack.Pop());
                    }
                    opStack.Pop();
                }
                else if (current is ArgumentSeparatorToken)
                {
                    while (opStack.TryPeek(out IToken? stackedToken) && ((stackedToken is OperatorToken) || (stackedToken is FunctionToken)))
                    {
                        postfix.Add(opStack.Pop());
                    }
                }
                else if (current is OperatorToken opToken)
                {
                    while (opStack.TryPeek(out IToken? stackedToken) &&
                        (((stackedToken is OperatorToken stackedOpToken) && (opToken.Operator.Precedence >= stackedOpToken.Operator.Precedence)) ||
                        (stackedToken is FunctionToken)))
                    {
                        postfix.Add(opStack.Pop());
                    }

                    opStack.Push(opToken);
                }
                else if (current is FunctionToken funcToken)
                {
                    opStack.Push(funcToken);
                }
                else
                {
                    // Something's wrong
                    throw new ExpressionException($"Unhandled token: {current}", current.Position, current.Length);
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
