using CalcBase.Numbers;

namespace CalcBase.Tokens
{
    /// <summary>
    /// Number token
    /// </summary>
    public record NumberToken : Token
    {
        /// <summary>
        /// Number
        /// </summary>
        public required Number Number { get; init; }
    }
}
