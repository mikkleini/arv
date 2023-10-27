using CalcBase.Functions;
using CalcBase.Operators;

namespace CalcBase.Tokens
{
    /// <summary>
    /// Function token
    /// </summary>
    public record FunctionToken : Token
    {
        /// <summary>
        /// Function
        /// </summary>
        public required IFunction Function { get; init; }

        /// <summary>
        /// Function argument count
        /// </summary>
        public int ArgumentCount
        {
            get
            {
                if (Function is INoArgumentFunction)
                {
                    return 0;
                }
                else if (Function is ISingleArgumentFunction)
                {
                    return 1;
                }
                else if (Function is IDualArgumentFunction)
                {
                    return 2;
                }
                else
                {
                    throw new NotImplementedException("Not supported function");
                }
            }
        }
    }
}
