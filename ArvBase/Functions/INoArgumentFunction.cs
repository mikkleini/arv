using ArvBase.Numbers;

namespace ArvBase.Functions
{
    public interface INoArgumentFunction : IFunction
    {
        IntegerRadix OutputRadix { get; }
        NumberType Calculate();
    }
}
