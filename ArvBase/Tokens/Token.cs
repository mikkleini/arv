namespace ArvBase.Tokens
{
    /// <summary>
    /// Token
    /// </summary>
    public abstract record Token : IToken
    {
        /// <summary>
        /// Position of the token in infix expression string
        /// </summary>
        public required int Position { get; init; }

        /// <summary>
        /// Length of the token in expression string
        /// </summary>
        public required int Length { get; init; }
    }
}
