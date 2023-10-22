namespace CalcBase.Operators.Bitwise
{
    public record OrOperator : IOperator
    {
        public string Symbol => "|";
        public string Name => "Bitwise OR";
        public int Precedence => 13;
        public OperatorAssociativity Associativity => OperatorAssociativity.Left;
        public bool IsUnary => false;
    }
}
