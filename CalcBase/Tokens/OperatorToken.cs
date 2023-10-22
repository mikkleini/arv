using CalcBase.Operators;

namespace CalcBase.Tokens
{
    /// <summary>
    /// Operator token
    /// </summary>
    public record OperatorToken : Token
    {
        public required IOperator Operator { get; init; }
    }
}
