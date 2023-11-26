using CalcBase.Numbers;

namespace CalcBase.Constants
{
    /// <summary>
    /// Constant
    /// </summary>
    public interface IConstant : IElement
    {
        string Name { get; }
        string Symbol { get; }
        string SimpleSymbol { get; }
        Number Number { get; }
    }
}
