using CalcBase.Operators;

namespace CalcBase.Tokens
{
    /// <summary>
    /// Operator token
    /// </summary>
    public record OperatorToken : Token
    {
        /// <summary>
        /// Operator
        /// </summary>
        public required IOperator Operator { get; init; }
    }
}
