using ArvBase.Quantities;

namespace ArvBase.Units
{
    /// <summary>
    /// Imperial unit
    /// </summary>
    public record ImperialUnit : IUnit
    {
        public UnitMultiple NominalMultiple => Multiples.Single(m => m.Factor == 1);
        public string Name => NominalMultiple.Name;
        public string[] Symbols => NominalMultiple.Symbols;
        public NumberType EqualSIValue { get; init; }
        public ISIUnit EqualSIUnit { get; init; }
        public IQuantity Quantity => EqualSIUnit.Quantity;
        public UnitMultiple[] Multiples { get; init; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="symbols">Symbol(s)</param>
        /// <param name="equalSIValue">Equal SI value</param>
        /// <param name="equalIUnit">Equal SI unit</param>
        /// <param name="multiples">Unit multiples</param>
        public ImperialUnit(NumberType equalSIValue, ISIUnit equalIUnit, UnitMultiple[] multiples)
        {
            EqualSIValue = equalSIValue;
            EqualSIUnit = equalIUnit;
            Multiples = multiples.Select(m => m with { Parent = this }).ToArray();
        }
    }
}
