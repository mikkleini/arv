namespace CalcBase.Functions
{
    public interface ISingleArgumentRealFunction : IFunction
    {
        RealType Calculate(RealType x);
    }
}
