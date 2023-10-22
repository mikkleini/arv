namespace CalcBase.Tokens
{
    /// <summary>
    /// Token
    /// </summary>
    public interface IToken
    {
        int Position { get; }
        int Length { get; }
    }
}
