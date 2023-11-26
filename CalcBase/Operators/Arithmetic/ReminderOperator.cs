namespace CalcBase.Operators.Arithmetic
{
    /// <summary>
    /// Remainder operator
    /// </summary>
    public record ReminderOperator : IBinaryOperator
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
        public NumberType Calculate(NumberType a, NumberType b)
        {
            return a % b;
        }
    }
}
