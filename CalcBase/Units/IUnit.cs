using CalcBase.Quantities;

namespace CalcBase.Units
{
    /// <summary>
    /// Interface for units
    /// </summary>
    public interface IUnit
    {
        string Name { get; }
        string Symbol { get; }
        string SimpleSymbol { get; }
        IQuantity Quantity { get; }
    }
}
