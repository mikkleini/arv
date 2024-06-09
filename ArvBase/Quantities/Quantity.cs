namespace ArvBase.Quantities
{
    /// <summary>
    /// Quantity
    /// </summary>
    public record Quantity : IQuantity
    {
        public string Name { get; init; }
        public string[] Symbols { get; init; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="symbols">Symbol(s)</param>
        public Quantity(string name, string[] symbols)
        {
            Name = name;
            Symbols = symbols;
        }
    }
}
