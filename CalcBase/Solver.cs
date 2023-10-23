using CalcBase.Generic;
using CalcBase.Operators;
using CalcBase.Tokens;
using CalcBase.Numbers;
using System.Numerics;

namespace CalcBase
{
    public class Solver
    {
        /// <summary>
        /// Convert integer to real
        /// </summary>
        /// <param name="i">Integer</param>
        /// <returns>Real</returns>
        private static RealType ConvertToReal(IntType i)
        {
            // TODO Error check
            return (decimal)i;
        }

        /// <summary>
        /// Convert real to integer
        /// </summary>
        /// <param name="i">Integer</param>
        /// <returns>Real</returns>
        private static IntType ConvertToInt(RealType i)
        {
            // TODO Error check
            return new IntType(i);
        }

        /// <summary>
        /// Try to convert number to integer number.
        /// Only works for real numbers if they don't have a fraction and are not using scientific notation.
        /// </summary>
        /// <param name="number">Number</param>
        /// <returns>If possible, integer number, otherwise null</returns>
        private static IntegerNumber? TryToConvertToInt(INumber number)
        {
            if (number is IntegerNumber intNumber)
            {
                return intNumber;
            }
            else if (number is RealNumber realNumber)
            {
                if (!realNumber.IsScientificNotation && RealType.IsInteger(realNumber.Value))
                {
                    return new IntegerNumber()
                    {
                        Value = ConvertToInt(realNumber.Value),
                        Radix = IntegerRadix.Unknown, // Will be determined when other operand is known
                        IsScientificNotation = realNumber.IsScientificNotation
                    };
                }
                else
                {
                    return null;
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Convert number to real number
        /// </summary>
        /// <param name="number">Number</param>
        /// <returns>If possible, integer number, otherwise null</returns>
        private static RealNumber ConvertToReal(INumber number)
        {
            if (number is RealNumber realNumber)
            {
                return realNumber;
            }
            else if (number is IntegerNumber intNumber)
            {
                return new RealNumber()
                {
                    Value = ConvertToReal(intNumber.Value),
                    IsScientificNotation = intNumber.IsScientificNotation
                };
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Vote the common radix
        /// </summary>
        /// <param name="a">Radix A</param>
        /// <param name="b">Radix B</param>
        /// <returns>Common radix</returns>
        private static IntegerRadix VoteCommonRadix(IntegerRadix a, IntegerRadix b)
        {
            return (a == IntegerRadix.Unknown ? b : a);
        }

        /// <summary>
        /// Vote the common dominant case (in case of hexadecimal numbers)
        /// </summary>
        /// <param name="a">Radix A</param>
        /// <param name="b">Radix B</param>
        /// <returns>Common radix</returns>
        private static DominantCase VoteCommonDominantCase(DominantCase a, DominantCase b)
        {
            return (a == DominantCase.None ? b : a);
        }

        private static INumber SolveOperation(Stack<INumber> numberStack, OperatorToken opToken)
        {
            IOperator op = opToken.Operator;

            if (op.OpCount == OperatorOpCountType.Unary)
            {
                INumber a = numberStack.Pop();

                // First check if integer number operation can be performed
                if (op is IUnaryIntegerOperation unaryIntOp)
                {
                    // Prefer integer operation
                    IntegerNumber? intA = TryToConvertToInt(a);
                    if (intA != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"  Operation {op.Name} with int {intA.Value}");
                        IntType value = unaryIntOp.Calculate(intA.Value);

                        return new IntegerNumber()
                        {
                            Value = value,
                            Radix = intA.Radix,
                            DominantCase = intA.DominantCase,
                            IsScientificNotation = intA.IsScientificNotation
                        };
                    }
                }

                // Then check for real number operation
                if (op is IUnaryRealOperation unaryRealOp)
                {
                    RealNumber realA = ConvertToReal(a);

                    System.Diagnostics.Debug.WriteLine($"  Operation {op.Name} with real {realA.Value}");
                    RealType value = unaryRealOp.Calculate(realA.Value);
                    
                    return new RealNumber()
                    {
                        Value = value,
                        IsScientificNotation = realA.IsScientificNotation
                    };
                }

                // Something's wrong
                throw new SolverException("Unsupported unary operation");
            }
            else if (op.OpCount == OperatorOpCountType.Binary)
            {
                INumber b = numberStack.Pop();
                INumber a = numberStack.Pop();

                // First check if integer number operation can be performed
                if (op is IBinaryIntegerOperation binIntOp)
                {
                    IntegerNumber? intA = TryToConvertToInt(a);
                    IntegerNumber? intB = TryToConvertToInt(b);

                    if ((intA != null) && (intB != null))
                    {
                        System.Diagnostics.Debug.WriteLine($"  Operation {op.Name} with int {intA.Value} and {intB.Value}");
                        IntType value = binIntOp.Calculate(intA.Value, intB.Value);
                        return new IntegerNumber()
                        {
                            Value = value,
                            Radix = VoteCommonRadix(intA.Radix, intB.Radix),
                            DominantCase = VoteCommonDominantCase(intA.DominantCase, intB.DominantCase),
                            IsScientificNotation = intA.IsScientificNotation || intB.IsScientificNotation
                        };
                    }
                }

                // Then check for real number operation
                if (op is IBinaryRealOperation binRealOp)
                {
                    RealNumber realA = ConvertToReal(a);
                    RealNumber realB = ConvertToReal(b);

                    System.Diagnostics.Debug.WriteLine($"  Operation {op.Name} with real {realA.Value} and {realB.Value}");
                    RealType value = binRealOp.Calculate(realA.Value, realB.Value);
                    return new RealNumber()
                    {
                        Value = value,
                        IsScientificNotation = realA.IsScientificNotation || realB.IsScientificNotation
                    };
                }

                // Something's wrong
                throw new SolverException("Unsupported binary operation");
            }
            else
            {
                // Something's wrong
                throw new SolverException("Unsupported operation");
            }
        }

        public static INumber Solve(IEnumerable<IToken> tokens)
        {
            Stack<INumber> numberStack = new();
            
            foreach (var token in tokens)
            {
                if (token is IntegerNumberToken intToken)
                {
                    numberStack.Push(intToken.Number);
                }
                else if (token is RealNumberToken realToken)
                {
                    numberStack.Push(realToken.Number);
                }
                else if (token is OperatorToken opToken)
                {
                    numberStack.Push(SolveOperation(numberStack, opToken));
                }
                else if (token is FunctionToken funcToken)
                {
                    // TODO
                }
            }

            return numberStack.Pop();
        }
    }
}
