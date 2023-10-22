namespace CalcBase.Operators.Bitwise
{
    public record AndOperator : IOperator
    {
        public string Symbol => "&";
        public string Name => "Bitwise AND";
        public int Precedence => 11;
        public OperatorAssociativity Associativity => OperatorAssociativity.Left;
        public bool IsUnary => false;
    }
}
