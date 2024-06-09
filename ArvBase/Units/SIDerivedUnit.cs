using ArvBase.Quantities;

namespace ArvBase.Units
{
    /// <summary>
    /// SI derived unit
    /// </summary>
    public record SIDerivedUnit : ISIUnit, IDerivedUnit
    {
        public UnitMultiple NominalMultiple => Multiples.Single(m => m.Factor == 1);
        public string Name => NominalMultiple.Name;
        public string[] Symbols => NominalMultiple.Symbols;
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
