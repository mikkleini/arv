namespace CalcBase.Tokens
{
    /// <summary>
    /// Parenthesis token
    /// </summary>
    public record ParenthesisToken : Token
    {
        public required ParenthesisSide Side { get; init; }
    }
}
