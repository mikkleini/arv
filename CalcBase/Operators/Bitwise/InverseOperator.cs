using CalcBase.Generic;

namespace CalcBase.Operators.Bitwise
{
    /// <summary>
    /// Inverse operator
    /// </summary>
    public record InverseOperator : IOperator, IUnaryIntegerOperation
    {
        public string Symbol => "~";
        public string Name => "Bitwise NOT";
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
            return ~a;
        }
    }
}
