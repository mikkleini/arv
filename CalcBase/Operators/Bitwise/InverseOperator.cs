namespace CalcBase.Operators.Bitwise
{
    public record InverseOperator : IOperator
    {
        public string Symbol => "~";
        public string Name => "Bitwise NOT";
        public int Precedence => 3;
        public OperatorAssociativity Associativity => OperatorAssociativity.Right;
        public bool IsUnary => true;
    }
}
