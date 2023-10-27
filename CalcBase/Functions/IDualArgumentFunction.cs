namespace CalcBase.Functions
{
    public interface IDualArgumentFunction : IFunction
    {
        NumberType Calculate(NumberType a, NumberType b);
    }
}
