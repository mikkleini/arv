using CalcBase.Generic;

namespace CalcBase.Operators.Bitwise
{
    /// <summary>
    /// Bitwise XOR operator
    /// </summary>
    public record XorOperator : IOperator, IBinaryIntegerOperation
    {
        public string Symbol => "^";
        public string Name => "Bitwise XOR";
        public int Precedence => 12;
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
            return a ^ b;
        }
    }
}
