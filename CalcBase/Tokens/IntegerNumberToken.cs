using CalcBase.Numbers;

namespace CalcBase.Tokens
{
    /// <summary>
    /// Integer number token
    /// </summary>
    public record IntegerNumberToken : IntegerNumber, IToken
    {
        public required int Position { get; init; }
        public required int Length { get; init; }
    }
}
