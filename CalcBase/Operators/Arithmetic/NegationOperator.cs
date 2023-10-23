using CalcBase.Generic;

namespace CalcBase.Operators.Arithmetic
{
    /// <summary>
    /// Negation operator
    /// </summary>
    public record NegationOperator : IOperator, IUnaryIntegerOperation, IUnaryRealOperation
    {
        public string Symbol => "-";
        public string Name => "Negation";
        public int Precedence => 3;
        public OperatorAssociativity Associativity => OperatorAssociativity.Right;
        public OperatorOpCountType OpCount => OperatorOpCountType.Unary;

        /// <summary>
        /// Calculate result of operation
        /// </summary>
        /// <param name="a">Operand A</param>
        /// <param name="requireRealOp">To require real number operation</param>
        /// <returns>Result of operation</returns>
        public IntType Calculate(IntType a, out bool requireRealOp)
        {
            requireRealOp = false;
            return -a;
        }

        /// <summary>
        /// Calculate result of operation
        /// </summary>
        /// <param name="a">Operand A</param>
        /// <returns>Result of operation</returns>
        public RealType Calculate(RealType a)
        {
            return -a;
        }
    }
}
