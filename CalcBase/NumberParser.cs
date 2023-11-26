using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using CalcBase.Numbers;
using CalcBase.Tokens;

[assembly: InternalsVisibleTo("CalcBaseTest")]
namespace CalcBase
{
    public partial class Parser 
    {
        /// <summary>
        /// Read binary number
        /// </summary>
        /// <param name="infix">Infix expression at number start</param>
        /// <param name="start">Number start position</param>
        /// <returns>Number token</returns>
        /// <exception cref="ExpressionException">Error in expression</exception>
        internal static NumberToken ReadBinaryNumber(ReadOnlySpan<char> infix, int start)
        {
            BigInteger value = 0;
            int pos;

            // Start after prefix
            for (pos = start + 2; pos < infix.Length; pos++)
            {
                char c = infix[pos];
                if (c == '0')
                {
                    value <<= 1;
                }
                else if (c == '1')
                {
                    value = (value << 1) | 1;
                }
                else
                {
                    break;
                }
            }

            // Length check
            if (pos - start < 3)
            {
                throw new ExpressionException("Incomplete binary number", start, pos - start);
            }

            if (pos - start > 2 + IntTypeBits)
            {
                throw new ExpressionException("Too large binary number", start, pos - start);
            }

            // Return token and end position
            return new NumberToken()
            {
                Position = start,
                Length = pos - start,
                Number = new Number(value, IntegerRadix.Binary)
            };
        }

        /// <summary>
        /// Read hexadecimal number
        /// </summary>
        /// <param name="infix">Infix expression at number start</param>
        /// <param name="start">Number start position</param>
        /// <returns>Number token</returns>
        /// <exception cref="ExpressionException">Error in expression</exception>
        internal static NumberToken ReadHexadecimalNumber(ReadOnlySpan<char> infix, int start)
        {
            BigInteger value = 0;
            int countLowerCase = 0;
            int countUpperCase = 0;
            int pos;

            // Start after prefix
            for (pos = start + 2; pos < infix.Length; pos++)
            {
                char c = infix[pos];
                if ((c >= '0') && (c <= '9'))
                {
                    value = (value << 4) | (BigInteger)(c - '0');
                }
                else if ((c >= 'a') && (c <= 'f'))
                {
                    value = (value << 4) | (BigInteger)(10 + (c - 'a'));
                    countLowerCase++;
                }
                else if ((c >= 'A') && (c <= 'F'))
                {
                    value = (value << 4) | (BigInteger)(10 + (c - 'A'));
                    countUpperCase++;
                }
                else
                {
                    break;
                }
            }

            // Length check
            if (pos - start < 3)
            {
                throw new ExpressionException("Incomplete hexadecimal number", start, pos - start);
            }

            if (pos - start > 2 + (IntTypeBits / 4))
            {
                throw new ExpressionException("Too large hexadecimal number", start, pos - start);
            }

            // Determine dominant hexadecimal letters case
            DominantHexadecimalCase dominantCase = DominantHexadecimalCase.None;
            if (countUpperCase > countLowerCase)
            {
                dominantCase = DominantHexadecimalCase.Upper;
            }
            else if (countUpperCase < countLowerCase)
            {
                dominantCase = DominantHexadecimalCase.Lower;
            }

            // Return token and end position
            return new NumberToken()
            {
                Position = start,
                Length = pos - start,
                Number = new Number(value, IntegerRadix.Hexadecimal, false, dominantCase)
            };
        }

        /// <summary>
        /// Read number
        /// </summary>
        /// <param name="infix">Infix expression</param>
        /// <param name="start">Number start position</param>
        /// <returns>Number token</returns>
        /// <exception cref="ExpressionException">Error in expression</exception>
        internal static NumberToken ReadNumber(ReadOnlySpan<char> infix, int start)
        {
            // Binary number ?
            if (infix.Slice(start).StartsWith(BinaryNumberPrefix.AsSpan()))
            {
                return ReadBinaryNumber(infix, start);
            }

            // Hexadecimal number ?
            else if (infix.Slice(start).StartsWith(HexadecimalNumberPrefix.AsSpan()))
            {
                return ReadHexadecimalNumber(infix, start);
            }

            // It could be integer or real number
            else
            {
                bool hasRadixPoint = false;
                bool hasExponent = false;
                int startExponent = 0;
                int lenNumber = 1;
                int lenExponent = 0;
                int pos;

                // First digit already read, start from second
                for (pos = start + 1; pos < infix.Length; pos++)
                {
                    char c = infix[pos];
                    if (char.IsDigit(c))
                    {
                        if (hasExponent)
                        {
                            lenExponent++;
                        }
                        else
                        {
                            lenNumber++;
                        }
                    }
                    else if (c == RadixPointSymbol)
                    {
                        if (hasExponent)
                        {
                            throw new ExpressionException("Exponent must be an integer", startExponent, pos - startExponent);
                        }
                        else if (hasRadixPoint)
                        {
                            throw new ExpressionException("Excessive radix point", pos, 1);
                        }

                        hasRadixPoint = true;
                        lenNumber++;
                    }
                    else if (c == ExponentPointSymbol)
                    {
                        if (hasExponent)
                        {
                            throw new ExpressionException("Excessive exponent notation", pos, 1);
                        }

                        hasExponent = true;
                        startExponent = pos + 1;
                    }
                    else if (hasExponent && (pos == startExponent) && ((c == '-') || (c == '+')))
                    {
                        // It's okay, it's the sign symbol
                        lenExponent++;
                    }
                    else
                    {
                        break;
                    }
                }

                // If there was exponent symbol, then there must be valid number behind it
                int exponent = 1;
                if (hasExponent)
                {
                    if ((lenExponent < 1) || ((lenExponent < 2) && ((infix[startExponent] == '-') || (infix[startExponent] == '+'))))
                    {
                        throw new ExpressionException("Missing exponent value", startExponent, 1);
                    }

                    // Check for overflow
                    try
                    {
                        exponent = int.Parse(infix.Slice(startExponent, lenExponent));
                    }
                    catch (OverflowException)
                    {
                        throw new ExpressionException("Too large exponent value", startExponent, lenExponent);
                    }
                }

                // Integer has no radix point and exponent is positive
                if (!hasRadixPoint && (exponent > 0))
                {
                    NumberType value = NumberType.Parse(infix.Slice(start, lenNumber));
                    if (hasExponent)
                    {
                        value *= NumberType.Pow(10, exponent);
                    }

                    // Return token and end position
                    return new NumberToken()
                    {
                        Position = start,
                        Length = pos - start,
                        Number = new Number(value, IntegerRadix.Decimal, hasExponent)
                    };
                }
                else
                {
                    NumberType value;

                    // Check for overflow
                    try
                    {
                        value = NumberType.Parse(infix.Slice(start, lenNumber), CultureInfo.InvariantCulture);
                    }
                    catch (OverflowException)
                    {
                        throw new ExpressionException("Too large decimal value", start, lenNumber);
                    }

                    if (hasExponent)
                    {
                        value *= NumberType.Pow(10, exponent);
                    }

                    // Return token and end position
                    return new NumberToken()
                    {
                        Position = start,
                        Length = pos - start,
                        Number = new Number(value, IntegerRadix.Decimal, hasExponent)
                    };
                }
            }
        }
    }
}
