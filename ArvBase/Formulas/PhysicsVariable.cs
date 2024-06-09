using ArvBase.Quantities;
using ArvBase.Units;

namespace ArvBase.Formulas
{
    /// <summary>
    /// Physics variable
    /// </summary>
    public record PhysicsVariable : IElement
    {
        public string Name { get; init; }
        public string[] Symbols { get; init; }
        public ISIUnit Unit { get; init; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="symbols">Symbol(s)</param>
        /// <param name="unit">SI unit</param>
        public PhysicsVariable(string name, string[] symbols, ISIUnit unit)
        {
            Name = name;
            Symbols = symbols;
            Unit = unit;
        }
    }
}
