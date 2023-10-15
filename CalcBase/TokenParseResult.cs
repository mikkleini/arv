using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcBase
{
    /// <summary>
    /// Token parse result
    /// </summary>
    public record TokenParseResult : Result<Token, ExpressionError>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="token">Token</param>
        public TokenParseResult(Token token)
        {
            Value = token;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="error">Expression error</param>
        public TokenParseResult(ExpressionError error)
        {
            Error = error;
        }
    }
}
