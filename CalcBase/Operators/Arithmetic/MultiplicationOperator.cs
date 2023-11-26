namespace CalcBase.Operators.Arithmetic
{
    /// <summary>
    /// Multiplication operator
    /// </summary>
    public record MultiplicationOperator : IBinaryOperator
    {
        public string Symbol { get; } = "*";
        public string Name { get; } = "Multiplication";
        public int Precedence { get; } = 5;
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
            return a * b;
        }        
    }
}
