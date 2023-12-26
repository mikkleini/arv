using CalcBase.Units;

namespace CalcBase.Tokens
{
    /// <summary>
    /// Unit token
    /// </summary>
    public record UnitToken : Token
    {
        /// <summary>
        /// Unit
        /// </summary>
        public required IUnit Unit { get; init; }

        /// <summary>
        /// Optional unit multiple
        /// </summary>
        public UnitMultiple? UnitMultiple { get; init; } = null;
    }
}
