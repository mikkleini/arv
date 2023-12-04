using CalcBase.Quantities;
using System.Diagnostics.CodeAnalysis;

namespace CalcBase.Units
{
    /// <summary>
    /// SI base unit
    /// </summary>
    public record SIBaseUnit : ISIBaseUnit
    {
        public string Name { get; init; }
        public string[] Symbols { get; init; }
        public IQuantity Quantity { get; init; }
        public UnitMultiple[] Multiples { get; init; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="symbols">Symbols</param>
        /// <param name="quantity">Quantity</param>
        public SIBaseUnit(string name, string[] symbols, IQuantity quantity)
        {
            Name = name;
            Symbols = symbols;
            Quantity = quantity;
            Multiples = [];
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="symbols">Symbols</param>
        /// <param name="quantity">Quantity</param>
        /// <param name="multiple">Unit multiples</param>
        public SIBaseUnit(string name, string[] symbols, IQuantity quantity, UnitMultiple[] multiple)
        {
            Name = name;
            Symbols = symbols;
            Quantity = quantity;
            Multiples = multiple;
        }
    }
}
