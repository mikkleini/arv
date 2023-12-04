using CalcBase.Quantities;

namespace CalcBase.Units
{
    /// <summary>
    /// SI derived unit
    /// </summary>
    public record SIDerivedUnit : ISIDerivedUnit
    {
        public string Name { get; init; }
        public string[] Symbols { get; init; }
        public IQuantity Quantity { get; init; }
        public IElement[] Expression { get; init; }
        public UnitMultiple[] Multiples { get; init; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="symbols">Symbol(s)</param>
        /// <param name="quantity">Quantity</param>
        /// <param name="expression">Expression</param>
        public SIDerivedUnit(string name, string[] symbols, IQuantity quantity, IElement[] expression)
        {
            Name = name;
            Symbols = symbols;
            Quantity = quantity;
            Expression = expression;
            Multiples = [];
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="symbols">Symbol(s)</param>
        /// <param name="quantity">Quantity</param>
        /// <param name="expression">Expression</param>
        /// <param name="multiples">Unit multiples</param>
        public SIDerivedUnit(string name, string[] symbols, IQuantity quantity, IElement[] expression, UnitMultiple[] multiples)
        {
            Name = name;
            Symbols = symbols;
            Quantity = quantity;
            Expression = expression;
            Multiples = multiples;
        }
    }
}
