using CalcBase.Generic;

namespace CalcBase.Operators.Arithmetic
{
    /// <summary>
    /// Negation operator
    /// </summary>
    public record NegationOperator : IOperator, IUnaryOperation
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
        /// <returns>Result of operation</returns>
        public NumberType Calculate(NumberType a)
        {
            return -a;
        }
    }
}
