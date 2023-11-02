namespace CalcBase.Tokens
{
    /// <summary>
    /// Token
    /// </summary>
    public interface IToken
    {
        /// <summary>
        /// Position of the token in infix expression string
        /// </summary>
        int Position { get; }

        /// <summary>
        /// Length of the token in expression string
        /// </summary>
        int Length { get; }
    }
}
