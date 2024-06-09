using ArvBase.Numbers;

namespace ArvBase.Tokens
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
