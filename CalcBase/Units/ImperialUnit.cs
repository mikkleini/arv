using CalcBase.Quantities;

namespace CalcBase.Units
{
    /// <summary>
    /// Imperial unit
    /// </summary>
    public record ImperialUnit : IImperialUnit
    {
        public string Name => Multiples.Single(m => m.Factor == 1).Name;
        public string[] Symbols => Multiples.Single(m => m.Factor == 1).Symbols;
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
            Multiples = multiples;
        }
    }
}
