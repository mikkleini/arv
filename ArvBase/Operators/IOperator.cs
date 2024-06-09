namespace ArvBase.Operators
{
    public interface IOperator : IElement
    {
        string Symbol { get; }
        string Name { get; }
        int Precedence { get; }
        OperatorAssociativity Associativity { get; }
        OperatorOpCountType OpCount { get; }
    }
}
