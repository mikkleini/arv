﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;

namespace CalcBase
{
    public partial class Parser 
    {
        /// <summary>
        /// Read binary number
        /// </summary>
        /// <param name="infix">Infix expression at number start</param>
        /// <param name="position">Read position</param>
        /// <returns>Binary number token</returns>
        /// <exception cref="ExpressionException">Error in expression</exception>
        private static Token ReadBinaryNumber(ReadOnlySpan<char> infix, int position)
        {
            StringBuilder text = new();
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
        private static Token ReadHexadecimalNumber(ReadOnlySpan<char> infix, int position)
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
        private static Token ReadNumber(ReadOnlySpan<char> infix, int position)
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
                bool hasRadixPoint = false;
                bool hasExponent = false;
                int lenNumber = 1;
                int startExponent = 0;
                int lenExponent = 0;

                // First digit already read, start from second
                for (int i = 1; i < infix.Length; i++)
                {
                    char c = infix[i];
                    if (char.IsDigit(c))
                    {
                        if (hasExponent)
                        {
                            lenNumber++;
                        }
                        else
                        {
                            lenExponent++;
                        }
                    }
                    else if (c == RadixPointSymbol)
                    {
                        if (hasExponent)
                        {
                            throw new ExpressionException("Exponent must be an integer", position, i);
                        }
                        else if (hasRadixPoint)
                        {
                            throw new ExpressionException("Excessive radix point", position, i);
                        }

                        hasRadixPoint = true;
                    }
                    else if (c == ExponentPointSymbol)
                    {
                        if (hasExponent)
                        {
                            throw new ExpressionException("Excessive exponent notation", position, i);
                        }

                        hasExponent = true;
                        startExponent = i + 1;
                    }
                    else if (hasExponent && (i == startExponent) && ((c == '-') || (c == '+')))
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
                IntType exponent = 1;
                if (hasExponent)
                {
                    if ((lenExponent < 1) || ((lenExponent < 2) && ((infix[startExponent] == '-') || (infix[startExponent] == '+'))))
                    {
                        throw new ExpressionException("Missing exponent value", position + startExponent, 1);
                    }

                    exponent = IntType.Parse(infix.Slice(startExponent, lenExponent));
                }

                // Is integer or real number ?
                if (!hasRadixPoint)
                {
                    IntType number = IntType.Parse(infix.Slice(0, lenNumber));

                    // Return token
                    return new IntegerNumberToken()
                    {
                        Length = lenNumber,
                        Value = number,
                        IsScientificNotation = hasExponent,
                        Exponent = exponent,
                    };
                }
                else
                {
                    RealType number = decimal.Parse(infix.Slice(0, lenNumber));

                    // Return token
                    return new RealNumberToken()
                    {
                        Length = lenNumber,
                        Value = number,
                        IsScientificNotation = hasExponent,
                        Exponent = exponent,
                    };
                }
            }
        }
    }
}
