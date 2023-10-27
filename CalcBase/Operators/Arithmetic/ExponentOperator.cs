﻿using CalcBase.Generic;
using System.Numerics;

namespace CalcBase.Operators.Arithmetic
{
    /// <summary>
    /// Exponent operator
    /// </summary>
    public record ExponentOperator : IOperator, IBinaryOperation
    {
        public string Symbol => "**";
        public string Name => "Exponent";
        public int Precedence => 4;
        public OperatorAssociativity Associativity => OperatorAssociativity.Left;
        public OperatorOpCountType OpCount => OperatorOpCountType.Binary;

        /// <summary>
        /// Calculate result of operation
        /// </summary>
        /// <param name="a">Operand A</param>
        /// <param name="b">Operand B</param>
        /// <returns>Result of operation</returns>
        public NumberType Calculate(NumberType a, NumberType b)
        {
            return NumberType.Pow(a, b);
        }
    }
}
