namespace CalcBase.Operators.Arithmetic
{
    /// <summary>
    /// Division operator
    /// </summary>
    public record DivisionOperator : Singleton<DivisionOperator>, IBinaryOperator
    {
        public string Symbol => "/";
        public string Name => "Division";
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
            return a / b;
        }
    }
}
