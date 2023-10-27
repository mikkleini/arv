namespace CalcBase.Functions
{
    public interface ISingleArgumentFunction : IFunction
    {
        NumberType Calculate(NumberType x);
    }
}
