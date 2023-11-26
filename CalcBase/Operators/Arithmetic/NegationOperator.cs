namespace CalcBase.Operators.Arithmetic
{
    /// <summary>
    /// Negation operator
    /// </summary>
    public record NegationOperator : IUnaryOperator
    {
        public string Symbol { get; } = "-";
        public string Name { get; } = "Negation";
        public int Precedence { get; } = 3;
        public OperatorAssociativity Associativity { get; } = OperatorAssociativity.Right;
        public OperatorOpCountType OpCount { get; } = OperatorOpCountType.Unary;

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
