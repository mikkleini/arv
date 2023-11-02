using CalcBase.Numbers;

namespace CalcBase.Functions
{
    public interface INoArgumentFunction : IFunction
    {
        IntegerRadix OutputRadix { get; }
        NumberType Calculate();
    }
}
