using ArvBase.Numbers;
using ArvBase.Units;

namespace ArvBase.Constants
{
    /// <summary>
    /// Constant
    /// </summary>
    public record Constant : IConstant
    {
        public string Name { get; init; }
        public string[] Symbols { get; init; }
        public Number Number { get; init; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="symbols">Symbol(s)</param>
        /// <param name="number">Number</param>
        public Constant(string name, string[] symbols, Number number)
        {
            Name = name;
            Symbols = symbols;
            Number = number;
        }
    }
}
