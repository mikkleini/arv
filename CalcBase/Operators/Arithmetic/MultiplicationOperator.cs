using CalcBase.Generic;

namespace CalcBase.Operators.Arithmetic
{
    /// <summary>
    /// Multiplication operator
    /// </summary>
    public record MultiplicationOperator : IOperator, IBinaryIntegerOperation, IBinaryRealOperation
    {
        public string Symbol => "*";
        public string Name => "Multiplication";
        public int Precedence => 5;
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
            return a * b;
        }

        /// <summary>
        /// Calculate result of operation
        /// </summary>
        /// <param name="a">Operand A</param>
        /// <param name="b">Operand B</param>
        /// <returns>Result of operation</returns>
        public RealType Calculate(RealType a, RealType b)
        {
            return a * b;
        }        
    }
}
