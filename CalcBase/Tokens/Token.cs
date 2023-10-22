namespace CalcBase.Tokens
{
    /// <summary>
    /// Token
    /// </summary>
    public abstract record Token : IToken
    {
        public int Position { get; init; }
        public int Length { get; init; }
    }
}
