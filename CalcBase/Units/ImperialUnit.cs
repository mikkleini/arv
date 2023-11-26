using CalcBase.Quantities;

namespace CalcBase.Units
{
    /// <summary>
    /// Imperial unit
    /// </summary>
    public record ImperialUnit : IImperialUnit
    {
        public string Name { get; init; }
        public string[] Symbols { get; init; }
        public NumberType EqualSIValue { get; init; }
        public ISIUnit EqualSIUnit { get; init; }
        public IQuantity Quantity => EqualSIUnit.Quantity;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="symbols">Symbol(s)</param>
        /// <param name="equalSIValue">Equal SI value</param>
        /// <param name="equalIUnit">Equal SI unit</param>
        public ImperialUnit(string name, string[] symbols, NumberType equalSIValue, ISIUnit equalIUnit)
        {
            Name = name;
            Symbols = symbols;
            EqualSIValue = equalSIValue;
            EqualSIUnit = equalIUnit;
        }
    }
}
