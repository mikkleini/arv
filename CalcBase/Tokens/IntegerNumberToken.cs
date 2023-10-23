using CalcBase.Numbers;

namespace CalcBase.Tokens
{
    /// <summary>
    /// Integer number token
    /// </summary>
    public record IntegerNumberToken : Token
    {
        /// <summary>
        /// Number
        /// </summary>
        public required IntegerNumber Number { get; init; }
    }
}
