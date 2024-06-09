namespace ArvBase.Operators.Arithmetic
{
    /// <summary>
    /// Subtraction operator
    /// </summary>
    public record SubtractionOperator : IBinaryOperator
    {
        public string Symbol { get; } = "-";
        public string Name { get; } = "Subtract";
        public int Precedence { get; } = 6;
        public OperatorAssociativity Associativity { get; } = OperatorAssociativity.Left;
        public OperatorOpCountType OpCount { get; } = OperatorOpCountType.Binary;

        /// <summary>
        /// Calculate result of operation
        /// </summary>
        /// <param name="a">Operand A</param>
        /// <param name="b">Operand B</param>
        /// <returns>Result of operation</returns>
        public NumberType Calculate(NumberType a, NumberType b)
        {
            return a - b;
        }
    }
}
