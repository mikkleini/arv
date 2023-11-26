using CalcBase.Quantities;

namespace CalcBase.Formulas
{
    /// <summary>
    /// Physics variable
    /// </summary>
    public record PhysicsVariable : IElement
    {
        public string Name { get; init; }
        public string[] Symbols { get; init; }
        public IQuantity Quantity { get; init; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="symbols">Symbol(s)</param>
        /// <param name="quantity">Quantity</param>
        public PhysicsVariable(string name, string[] symbols, IQuantity quantity)
        {
            Name = name;
            Symbols = symbols;
            Quantity = quantity;
        }
    }
}
