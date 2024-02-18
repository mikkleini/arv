using System;
using CalcBase.Operators.Arithmetic;
using System.Numerics;

namespace CalcBase.Operators.Bitwise
{
    /// <summary>
    /// Bitwise right shift operator
    /// </summary>
    public record RightShiftOperator : IBinaryOperator
    {
        public string Symbol { get; } = ">>";
        public string Name { get; } = "Bitwise right shift";
        public int Precedence { get; } = 5;
        public OperatorAssociativity Associativity { get; } = OperatorAssociativity.Left;
        public OperatorOpCountType OpCount { get; } = OperatorOpCountType.Binary;

        /// <summary>
        /// Calculate result of operation
        /// </summary>
        /// <param name="a">Operand A</param>
        /// <param name="b">Operand B</param>
        /// <returns>Result of operation</returns>
        public NumberType Calculate(NumberType a, NumberType b)
        {
            if (!a.IsNatural())
            {
                throw new SolverException($"{Name} can only be performed on natural number");
            }

            if (!b.IsNatural())
            {
                throw new SolverException($"{Name} can only be performed with natural number");
            }

            return new NumberType(((BigInteger)a) >> ((int)b));
        }
    }
}
