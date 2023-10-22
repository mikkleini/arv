namespace CalcBase.Operators.Arithmetic
{
    public record QuotientOperator : IOperator
    {
        public string Symbol => "//";
        public string Name => "Quotient";
        public int Precedence => 4;
        public OperatorAssociativity Associativity => OperatorAssociativity.Left;
        public bool IsUnary => false;
    }
}
