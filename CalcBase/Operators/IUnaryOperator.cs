namespace CalcBase.Operators
{
    public interface IUnaryOperator : IOperator
    {
        NumberType Calculate(NumberType a);
    }
}
