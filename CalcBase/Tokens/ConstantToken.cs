using CalcBase.Constants;

namespace CalcBase.Tokens
{
    /// <summary>
    /// Constant token
    /// </summary>
    public record ConstantToken : Token
    {
        /// <summary>
        /// Constant
        /// </summary>
        public required IConstant Constant { get; init; }
    }
}
