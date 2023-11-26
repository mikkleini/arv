using CalcBase.Quantities;

namespace CalcBase.Units
{
    /// <summary>
    /// Interface for units
    /// </summary>
    public interface IUnit : IElement
    {
        string Name { get; }
        string[] Symbols { get; }
        IQuantity Quantity { get; }
    }
}
