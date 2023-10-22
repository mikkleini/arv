using CalcBase.Generic;

namespace CalcBase.Operators.Arithmetic
{
    /// <summary>
    /// Modulus operator
    /// </summary>
    public record ModulusOperator : IOperator, IBinaryIntegerOperation, IBinaryRealOperation
    {
        public string Symbol => "%";
        public string Name => "Modulus";
        public int Precedence => 5;
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
            return a % b;
        }

        /// <summary>
        /// Calculate result of operation
        /// </summary>
        /// <param name="a">Operand A</param>
        /// <param name="b">Operand B</param>
        /// <returns>Result of operation</returns>
        public RealType Calculate(RealType a, RealType b)
        {
            return a % b;
        }
    }
}
