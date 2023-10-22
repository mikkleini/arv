namespace CalcBase.Operators.Arithmetic
{
    public record ExponentOperator : IOperator
    {
        public string Symbol => "**";
        public string Name => "Exponent";
        public int Precedence => 4;
        public OperatorAssociativity Associativity => OperatorAssociativity.Left;
        public bool IsUnary => false;
    }
}
