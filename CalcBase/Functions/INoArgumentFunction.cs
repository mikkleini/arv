using CalcBase.Tokens;

namespace CalcBase.Functions
{
    public interface INoArgumentFunction : IFunction
    {
        IntegerRadix OutputRadix { get; }
        NumberType Calculate();
    }
}
