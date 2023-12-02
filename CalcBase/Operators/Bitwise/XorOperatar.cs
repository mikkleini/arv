﻿using System.Numerics;

namespace CalcBase.Operators.Bitwise
{
    /// <summary>
    /// Bitwise XOR operator
    /// </summary>
    public record XorOperator : IBinaryOperator
    {
        public string Symbol { get; } = "^";
        public string Name { get; } = "Bitwise XOR";
        public int Precedence { get; } = 12;
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
                throw new SolverException($"{Name} can only be performed with natural numbers");
            }

            if (!b.IsNatural())
            {
                throw new SolverException($"{Name} can only be performed with natural numbers");
            }

            return new NumberType(((BigInteger)a) ^ ((BigInteger)b));
        }
    }
}
