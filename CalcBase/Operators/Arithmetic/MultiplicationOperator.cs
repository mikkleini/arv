namespace CalcBase.Operators.Arithmetic
{
    public record MultiplicationOperator : IOperator
    {
        public string Symbol => "*";
        public string Name => "Multiplication";
        public int Precedence => 5;
        public OperatorAssociativity Associativity => OperatorAssociativity.Left;
        public bool IsUnary => false;
    }
}
