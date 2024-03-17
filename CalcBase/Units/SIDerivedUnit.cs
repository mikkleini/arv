using CalcBase.Quantities;

namespace CalcBase.Units
{
    /// <summary>
    /// SI derived unit
    /// </summary>
    public record SIDerivedUnit : ISIDerivedUnit
    {
        public string Name => Multiples.Single(m => m.Factor == 1).Name;
        public string[] Symbols => Multiples.Single(m => m.Factor == 1).Symbols;
        public IQuantity Quantity { get; init; }
        public IElement[] Expression { get; init; }
        public UnitMultiple[] Multiples { get; init; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="quantity">Quantity</param>
        /// <param name="expression">Expression</param>
        /// <param name="multiples">Unit multiples</param>
        public SIDerivedUnit(IQuantity quantity, IElement[] expression, UnitMultiple[] multiples)
        {
            Quantity = quantity;
            Expression = expression;
            Multiples = multiples.Select(m => m with { Parent = this }).ToArray();
        }
    }
}
