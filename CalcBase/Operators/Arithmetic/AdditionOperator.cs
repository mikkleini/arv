namespace CalcBase.Operators.Arithmetic
{
    public record AdditionOperator : IOperator
    {
        public string Symbol => "+";
        public string Name => "Addition";
        public int Precedence => 6;
        public OperatorAssociativity Associativity => OperatorAssociativity.Left;
        public bool IsUnary => false;
    }
}
