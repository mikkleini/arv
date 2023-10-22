namespace CalcBase.Operators.Arithmetic
{
    public record ModulusOperator : IOperator
    {
        public string Symbol => "%";
        public string Name => "Modulus";
        public int Precedence => 5;
        public OperatorAssociativity Associativity => OperatorAssociativity.Left;
        public bool IsUnary => false;
    }
}
