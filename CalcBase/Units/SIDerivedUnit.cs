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
        public (string[] symbols, string name, NumberType weight)[] Weights { get; init; }

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
            Weights = [];
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="symbols">Symbol(s)</param>
        /// <param name="quantity">Quantity</param>
        /// <param name="expression">Expression</param>
        /// <param name="weights">Unit weights</param>
        public SIDerivedUnit(string name, string[] symbols, IQuantity quantity, IElement[] expression, (string[] symbols, string name, NumberType weight)[] weights)
        {
            Name = name;
            Symbols = symbols;
            Quantity = quantity;
            Expression = expression;
            Weights = weights;
        }
    }
}
