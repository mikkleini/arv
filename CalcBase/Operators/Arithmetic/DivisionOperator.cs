namespace CalcBase.Operators.Arithmetic
{
    public record DivisionOperator : IOperator
    {
        public string Symbol => "/";
        public string Name => "Division";
        public int Precedence => 5;
        public OperatorAssociativity Associativity => OperatorAssociativity.Left;
        public bool IsUnary => false;
    }
}
