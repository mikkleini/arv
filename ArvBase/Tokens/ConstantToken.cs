using ArvBase.Constants;

namespace ArvBase.Tokens
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
