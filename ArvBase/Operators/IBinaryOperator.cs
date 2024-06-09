namespace ArvBase.Operators
{
    public interface IBinaryOperator : IOperator
    {
        NumberType Calculate(NumberType a, NumberType b);
    }
}
