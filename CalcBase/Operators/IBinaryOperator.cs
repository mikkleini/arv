namespace CalcBase.Operators
{
    public interface IBinaryOperator : IOperator
    {
        NumberType Calculate(NumberType a, NumberType b);
    }
}
