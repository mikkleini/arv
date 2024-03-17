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
        public required UnitMultiple Unit { get; init; }
    }
}
