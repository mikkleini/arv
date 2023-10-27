using CalcBase.Numbers;

namespace CalcBase.Constants
{
    public interface IConstant
    {
        string Name { get; }
        string Symbol { get; }
        string SimpleSymbol { get; }
        Number Number { get; }
    }
}
