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
        public required UnitMultiple UnitMultiple { get; init; }
    }
}
