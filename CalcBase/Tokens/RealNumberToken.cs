using CalcBase.Numbers;

namespace CalcBase.Tokens
{
    /// <summary>
    /// Real number token
    /// </summary>
    public record RealNumberToken : RealNumber, IToken
    {
        public required int Position { get; init; }
        public required int Length { get; init; }
    }
}
