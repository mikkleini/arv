using ArvBase.Operators;

namespace ArvBase.Tokens
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
