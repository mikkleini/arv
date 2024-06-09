using ArvBase.Quantities;
using System.Diagnostics.CodeAnalysis;

namespace ArvBase.Units
{
    /// <summary>
    /// SI base unit
    /// </summary>
    public record SIBaseUnit : ISIUnit
    {
        public UnitMultiple NominalMultiple => Multiples.Single(m => m.Factor == 1);
        public string Name => NominalMultiple.Name;
        public string[] Symbols => NominalMultiple.Symbols;
        public IQuantity Quantity { get; init; }
        public UnitMultiple[] Multiples { get; init; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="quantity">Quantity</param>
        /// <param name="multiples">Unit multiples</param>
        public SIBaseUnit(IQuantity quantity, UnitMultiple[] multiples)
        {
            Quantity = quantity;
            Multiples = multiples.Select(m => m with { Parent = this }).ToArray();
        }
    }
}
