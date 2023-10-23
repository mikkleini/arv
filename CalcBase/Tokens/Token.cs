namespace CalcBase.Tokens
{
    /// <summary>
    /// Token
    /// </summary>
    public abstract record Token : IToken
    {
        /// <summary>
        /// Token position in infix expression
        /// </summary>
        public int Position { get; init; }

        /// <summary>
        /// Token length in infix expression
        /// </summary>
        public int Length { get; init; }
    }
}
