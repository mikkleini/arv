using ArvBase.Quantities;

namespace ArvBase.Units
{
    /// <summary>
    /// Interface for units
    /// </summary>
    public interface IUnit : IElement
    {
        string Name { get; }
        string[] Symbols { get; }
        IQuantity Quantity { get; }
        UnitMultiple[] Multiples { get; }
        UnitMultiple NominalMultiple { get; }
    }
}
