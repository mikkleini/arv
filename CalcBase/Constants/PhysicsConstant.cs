using CalcBase.Numbers;
using CalcBase.Quantities;
using CalcBase.Units;

namespace CalcBase.Constants
{
    /// <summary>
    /// Physics constant
    /// </summary>
    public record PhysicsConstant : IPhysicsConstant
    {
        public string Name { get; init; }
        public string[] Symbols { get; init; }
        public Number Number { get; init; }
        public IUnit Unit { get; init; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="symbols">Symbol(s)</param>
        /// <param name="number">Number</param>
        /// <param name="unit">Unit</param>
        public PhysicsConstant(string name, string[] symbols, Number number, IUnit unit)
        {
            Name = name;
            Symbols = symbols;
            Number = number;
            Unit = unit;
        }
    }
}
