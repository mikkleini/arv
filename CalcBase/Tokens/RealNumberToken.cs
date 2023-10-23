using CalcBase.Numbers;

namespace CalcBase.Tokens
{
    /// <summary>
    /// Real number token
    /// </summary>
    public record RealNumberToken : Token
    {
        /// <summary>
        /// Number
        /// </summary>
        public required RealNumber Number { get; init; }
    }
}
