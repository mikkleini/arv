using System.ComponentModel.Design;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using CalcBase.Operators;

namespace CalcBase
{
    public class Parser
    {
        private static readonly string BinaryNumberPrefix = "0b";
        private static readonly string HexadecimalNumberPrefix = "0x";
        private static readonly char RadixPointSymbol = '.';
        private static readonly char ArgumentSeparatorSymbol = ',';
        private static readonly int IntTypeBits = 128;

        private static readonly IDictionary<string, Operator> operators = new Dictionary<string, Operator>
        {
            ["+"] = new Operator { Name = "+", Precedence = 1 },
            ["-"] = new Operator { Name = "-", Precedence = 1 },
            ["*"] = new Operator { Name = "*", Precedence = 2 },
            ["/"] = new Operator { Name = "/", Precedence = 2 },
            ["^"] = new Operator { Name = "^", Precedence = 3, RightAssociative = true }
        };

        /// <summary>
        /// Read binary number
        /// </summary>
        /// <param name="infix">Infix expression at number start</param>
        /// <param name="position">Read position</param>
        /// <returns>Binary number token</returns>
        /// <exception cref="ExpressionException">Error in expression</exception>
        private Token ReadBinaryNumber(ReadOnlySpan<char> infix, int position)
        {
            StringBuilder text = new StringBuilder();
            IntType value = 0;
            int i;

            // Start after prefix
            for (i = 2; i < infix.Length; i++)
            {
                char c = infix[i];
                if (c == '0')
                {
                    value <<= 1;
                    text.Append(c);
                }
                else if (c == '1')
                {
                    value = (value << 1) | 1;
                    text.Append(c);
                }
                else
                {
                    break;
                }
            }

            // Length check
            if (i > 2 + (IntTypeBits - 1))
            {
                throw new ExpressionException("Too large binary number", position, i);
            }

            // Return token
            return new BinaryNumberToken()
            {
                Type = TokenType.BinaryNumber,
                Length = i,
                Value = value                
            };
        }

        /// <summary>
        /// Read hexadecimal number
        /// </summary>
        /// <param name="infix">Infix expression at number start</param>
        /// <param name="position">Read position</param>
        /// <returns>Hexadecimal number token</returns>
        /// <exception cref="ExpressionException">Error in expression</exception>
        private Token ReadHexadecimalNumber(ReadOnlySpan<char> infix, int position)
        {
            IntType value = 0;
            int countLowerCase = 0;
            int countUpperCase = 0;
            int i;

            // Start after prefix
            for (i = 2; i < infix.Length; i++)
            {
                char c = infix[i];
                if ((c >= '0') && (c <= '9'))
                {
                    value = (value << 4) | (IntType)(c - '0');
                }
                else if ((c >= 'a') && (c <= 'f'))
                {
                    value = (value << 4) | (IntType)(10 + (c - 'a'));
                    countLowerCase++;
                }
                else if ((c >= 'A') && (c <= 'F'))
                {
                    value = (value << 4) | (IntType)(10 + (c - 'A'));
                    countUpperCase++;
                }
                else
                {
                    break;
                }
            }

            // Length check
            if (i > 2 + ((IntTypeBits - 1) / 4))
            {
                throw new ExpressionException("Too large hexadecimal number", position, i);
            }

            // Determine dominant hexadecimal letters case
            DominantCase dominantCase = DominantCase.None;
            if (countUpperCase > countLowerCase)
            {
                dominantCase = DominantCase.Upper;
            }
            else if (countUpperCase < countLowerCase)
            {
                dominantCase = DominantCase.Lower;
            }

            // Return token
            return new HexadecimalNumberToken()
            {
                Type = TokenType.HexadecimalNumber,
                Length = i,
                Value = value,
                DominantCase = dominantCase
            };
        }

        /// <summary>
        /// Read number
        /// </summary>
        /// <param name="infix">Infix expression</param>
        /// <param name="position">Search position</param>
        /// <returns>Some number token</returns>
        /// <exception cref="ExpressionException">Error in expression</exception>
        private Token ReadNumber(ReadOnlySpan<char> infix, int position)
        {
            // Binary number ?
            if (infix.StartsWith(BinaryNumberPrefix.AsSpan()))
            {
                return ReadBinaryNumber(infix, position);
            }

            // Hexadecimal number ?
            else if (infix.StartsWith(HexadecimalNumberPrefix.AsSpan()))
            {
                return ReadHexadecimalNumber(infix, position);
            }

            // It could be integer or real number
            else
            {
                int numRadix = 0;
                int i;

                // First digit already read, start from second
                for (i = 1; i < infix.Length; i++)
                {
                    char c = infix[i];
                    if (char.IsDigit(c))
                    {
                        // Continue
                    }
                    else if (c == RadixPointSymbol)
                    {
                        numRadix++;
                        if (numRadix > 1)
                        {
                            throw new ExpressionException("Excessive radix point", position, i);
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                int numberLen = i;

                // Is integer or real number ?
                if (numRadix == 0)
                {
                    IntType value = IntType.Parse(infix[0..numberLen]);

                    // Return token
                    return new IntegerNumberToken()
                    {
                        Type = TokenType.IntegerNumber,
                        Length = numberLen,
                        Value = value
                    };
                }
                else
                {
                    RealType value = decimal.Parse(infix[0..numberLen]);

                    // Return token
                    return new RealNumberToken()
                    {
                        Type = TokenType.RealNumber,
                        Length = numberLen,
                        Value = value
                    };
                }
            }
        }

        /// <summary>
        /// Read function or variable
        /// </summary>
        /// <param name="infix">Infix expression</param>
        /// <param name="position">Search position</param>
        /// <returns>Function or variable token</returns>
        /// <exception cref="ExpressionException">Error in expression</exception>
        private Token ReadFunctionOrVariable(ReadOnlySpan<char> infix, int position)
        {
            StringBuilder text = new StringBuilder();

            for (int i = 1; i < infix.Length; i++)
            {
                char c = infix[i];

                if (char.IsLetterOrDigit(c))
                {
                    text.Append(c);
                }
                else if (c == '(')
                {
                    // It's a function
                }
                else if (char.IsWhiteSpace(c))
                {
                }
            }
        }

        public void ShuntingYard(string infix)
        {
            ReadOnlySpan<char> span = infix.AsSpan();
            Token token;


            for (int i = 0; i < span.Length; i++) {
            {
                char c = span[i];

                if (char.IsWhiteSpace(c))
                {
                    // Ignore
                }
                else if (char.IsDigit(c))
                {
                    token = ReadNumber(span[i..], i);
                }
                else if (char.IsLetter(c))
                {
                    token = ReadFunctionOrVariable(span[i..], i);
                }                
            }
        }
    }
}
