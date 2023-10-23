using CalcBase.Generic;

namespace CalcBase.Operators.Bitwise
{
    /// <summary>
    /// Bitwise OR operator
    /// </summary>
    public record OrOperator : IOperator, IBinaryIntegerOperation
    {
        public string Symbol => "|";
        public string Name => "Bitwise OR";
        public int Precedence => 13;
        public OperatorAssociativity Associativity => OperatorAssociativity.Left;
        public OperatorOpCountType OpCount => OperatorOpCountType.Binary;

        /// <summary>
        /// Calculate result of operation
        /// </summary>
        /// <param name="a">Operand A</param>
        /// <param name="b">Operand B</param>
        /// <param name="requireRealOp">To require real number operation</param>
        /// <returns>Result of operation</returns>
        public IntType Calculate(IntType a, IntType b, out bool requireRealOp)
        {
            requireRealOp = false;
            return a | b;
        }
    }
}
