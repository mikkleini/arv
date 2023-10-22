using System.Runtime.CompilerServices;
using System.Text;
using CalcBase.Operators;
using CalcBase.Operators.Arithmetic;
using CalcBase.Operators.Bitwise;

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
        private readonly IOperator[] operators = new IOperator[]
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

        /// <summary>
        /// Read text (don't yet know if it's constant, unit, variable or function)
        /// </summary>
        /// <param name="infix">Infix expression</param>
        /// <param name="position">Search position</param>
        /// <returns>Text token</returns>
        /// <exception cref="ExpressionException">Error in expression</exception>
        private static Token ReadText(ReadOnlySpan<char> infix, int position)
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

            return new TextToken()
            {
                Position = position,
                Length = text.Length,
                Text = text.ToString()
            };
        }

        /// <summary>
        /// Shunting yard algoritm to get RPN expression of tokens
        /// </summary>
        /// <param name="infix">Infix epxression</param>
        /// <returns>List of tokens</returns>
        /// <exception cref="ExpressionException">Exception in infix expression</exception>
        public List<Token> ShuntingYard(string infix)
        {
            ReadOnlySpan<char> span = infix.AsSpan();
            List<Token> tokens = new();
            int i = 0;

            while (i < span.Length)
            {
                char c = span[i];
                Token? token = null;

                if (char.IsWhiteSpace(c))
                {
                    // Ignore
                }
                else if (char.IsDigit(c))
                {
                    token = ReadNumber(span.Slice(i), i);
                }
                else if (char.IsLetter(c))
                {
                    token = ReadText(span.Slice(i), i);
                }
                else if (c == '(')
                {
                    token = new ParenthesisToken()
                    {
                        Length = 1,
                        Side = Side.Left
                    };
                }
                else if (c == ')')
                {
                    token = new ParenthesisToken()
                    {
                        Length = 1,
                        Side = Side.Right
                    };
                }
                else if (c == '-')
                {
                    // Minus operator requires special handling
                    bool isNegation = false;

                    if (tokens.Count == 0)
                    {
                        isNegation = true;
                    }
                    else
                    {
                        Token prevToken = tokens.Last();
                        
                        if ((prevToken.Type == TokenType.Parenthesis) && (((ParenthesisToken)prevToken).Side == Side.Left))
                        {
                            isNegation = true;
                        }
                        else if (prevToken.Type == TokenType.Operator)
                        {
                            isNegation = true;
                        }
                    }

                    // So which operator it is
                    if (isNegation)
                    {
                        token = new OperatorToken()
                        {
                            Position = i,
                            Length = 1,
                            Operator = operators.Single(op => op is NegationOperator),
                        };
                    }
                    else
                    {
                        token = new OperatorToken()
                        {
                            Position = i,
                            Length = 1,
                            Operator = operators.Single(op => op is SubtractionOperator),
                        };
                    }
                }
                else
                {
                    // Check for operators by length of the symbol.
                    // Basically if expression contains ** then it should first try to find exponent operator **, not the multiplication.
                    IOperator? op = operators
                        .OrderByDescending(o => o.Symbol.Length)
                        .FirstOrDefault(o => infix.AsSpan().Slice(i).StartsWith(o.Symbol.AsSpan()));

                    // Found operator ?
                    if (op != null)
                    {
                        token = new OperatorToken()
                        {
                            Operator = op,
                            Length = op.Symbol.Length
                        };
                    }
                }

                // Got token ?
                if (token != null)
                {
                    i += token.Length;
                    tokens.Add(token);
                }
                else
                {
                    throw new ExpressionException("Syntax error", i, span.Length - i);
                }
            }

            return tokens;
        }
    }
}
