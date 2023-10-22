using CalcBase.Generic;

namespace CalcBase.Operators.Arithmetic
{
    /// <summary>
    /// Quotient operator
    /// </summary>
    public record QuotientOperator : IOperator, IBinaryIntegerOperation, IBinaryRealOperation
    {
        public string Symbol => "//";
        public string Name => "Quotient";
        public int Precedence => 4;
        public OperatorAssociativity Associativity => OperatorAssociativity.Left;
        public OperatorOpCountType OpCount => OperatorOpCountType.Binary;

        /// <summary>
        /// Calculate result of operation
        /// </summary>
        /// <param name="a">Operand A</param>
        /// <param name="b">Operand B</param>
        /// <returns>Result of operation</returns>
        public IntType Calculate(IntType a, IntType b)
        {
            return a / b;
        }

        /// <summary>
        /// Calculate result of operation
        /// </summary>
        /// <param name="a">Operand A</param>
        /// <param name="b">Operand B</param>
        /// <returns>Result of operation</returns>
        public RealType Calculate(RealType a, RealType b)
        {
            // TODO Return IntType ?
            return decimal.Floor(a / b);
        }
    }
}
