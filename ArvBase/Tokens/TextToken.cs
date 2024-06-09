namespace ArvBase.Tokens
{
    /// <summary>
    /// Text token
    /// </summary>
    public record TextToken : Token
    {
        public required string Text { get; init; }
    }
}
