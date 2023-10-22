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
        /// <param name="end">Number end position</param>
        /// <returns>Text token</returns>
        /// <exception cref="ExpressionException">Error in expression</exception>
        internal static IToken ReadText(ReadOnlySpan<char> infix, int start, out int end)
        {
            StringBuilder text = new();

            for (int i = 0; i < infix.Length; i++)
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
            end = start + text.Length;
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
        internal static IOperator FindMinusOperator(IToken? prevToken)
        {
            bool isNegation = false;

            if (prevToken == null)
            {
                isNegation = true;
            }
            else
            {
                if ((prevToken is ParenthesisToken token) && (token.Side == ParenthesisSide.Left))
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
        /// Shunting yard algoritm to get RPN expression of tokens
        /// </summary>
        /// <param name="infix">Infix epxression</param>
        /// <returns>List of tokens</returns>
        /// <exception cref="ExpressionException">Exception in infix expression</exception>
        public static List<IToken> ShuntingYard(string infix)
        {
            List<IToken> tokens = new();
            Stack<IToken> operatorStack = new();
            int i = 0;

            while (i < infix.Length)
            {
                char c = infix[i];

                if (char.IsWhiteSpace(c))
                {
                    // Ignore
                    i++;
                }
                else if (char.IsDigit(c))
                {
                    tokens.Add(ReadNumber(infix, i, out i));
                }
                else if (char.IsLetter(c))
                {
                    tokens.Add(ReadText(infix, i, out i));
                }
                else if (c == '(')
                {
                    operatorStack.Push(new ParenthesisToken()
                    {
                        Position = i,
                        Length = 1,
                        Side = ParenthesisSide.Left
                    });
                    i++;
                }
                else if (c == ')')
                {
                    while (operatorStack.TryPop(out IToken? token))
                    {
                        if ((token is ParenthesisToken parenthesisToken) && (parenthesisToken.Side == ParenthesisSide.Left))
                        {
                            break;
                        }
                        tokens.Add(token);
                    }
                    i++;
                }
                else
                {
                    IOperator? op;

                    // Special case with minus operator
                    if (c == '-')
                    {
                        op = FindMinusOperator(tokens.LastOrDefault());
                    }
                    else
                    {
                        // Check for operators by length of the symbol.
                        // Basically if expression contains ** then it should first try to find exponent operator **, not the multiplication.
                        op = operators
                            .OrderByDescending(o => o.Symbol.Length)
                            .FirstOrDefault(o => infix.AsSpan().Slice(i).StartsWith(o.Symbol.AsSpan()));
                    }

                    // Found operator ?
                    if (op != null)
                    {
                        var opToken = new OperatorToken()
                        {
                            Position = i,
                            Length = op.Symbol.Length,
                            Operator = op
                        };

                        while (operatorStack.TryPeek(out IToken? token))
                        {
                            if ((token is ParenthesisToken parenthesisToken) && (parenthesisToken.Side == ParenthesisSide.Left))
                            {
                                break;
                            }
                            OperatorToken opToken2 = (OperatorToken)token;

                            if (opToken.Operator.Precedence > opToken2.Operator.Precedence)
                            {
                                //break;
                                System.Diagnostics.Debug.WriteLine($"Op to queue {opToken2}");
                                tokens.Add(operatorStack.Pop());
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine($"Op not to queue");
                            }

                            //tokens.Add(operatorStack.Pop());                            
                        }

                        operatorStack.Push(opToken);
                        i += opToken.Length;
                    }
                    else
                    {
                        throw new ExpressionException("Syntax error", i, infix.Length - i);
                    }
                }
            }

            while (operatorStack.TryPop(out IToken? opToken))
            {
                tokens.Add(opToken);
            }

            return tokens;
        }
    }
}
