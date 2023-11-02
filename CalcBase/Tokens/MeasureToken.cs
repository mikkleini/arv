using CalcBase.Numbers;

namespace CalcBase.Tokens
{
    /// <summary>
    /// Measure token
    /// </summary>
    public record MeasureToken : Token
    {
        /// <summary>
        /// Measure
        /// </summary>
        public required Measure Measure { get; init; }
    }
}
