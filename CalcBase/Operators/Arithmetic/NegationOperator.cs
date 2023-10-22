namespace CalcBase.Operators.Arithmetic
{
    public record NegationOperator : IOperator
    {
        public string Symbol => "-";
        public string Name => "Negation";
        public int Precedence => 3;
        public OperatorAssociativity Associativity => OperatorAssociativity.Right;
        public bool IsUnary => true;
    }
}
