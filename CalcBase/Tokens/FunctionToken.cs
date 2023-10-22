using CalcBase.Functions;
using CalcBase.Operators;

namespace CalcBase.Tokens
{
    /// <summary>
    /// Function token
    /// </summary>
    public record FunctionToken : Token
    {
        public required IFunction Function { get; init; }
    }
}
