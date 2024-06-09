namespace ArvBase.Operators
{
    public interface IUnaryOperator : IOperator
    {
        NumberType Calculate(NumberType a);
    }
}
