using CalcBase.Tokens;

namespace CalcBase
{
    /// <summary>
    /// Expression exception
    /// </summary>
    public class ExpressionException : Exception
    {
        /// <summary>
        /// Erroneous expression position
        /// </summary>
        public int Position { get; init; }

        /// <summary>
        /// Erroneous expression length
        /// </summary>
        public int Length { get; init; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="position">Erroneous expression position</param>
        /// <param name="length">Erroneous expression length</param>
        public ExpressionException(string message, int position, int length)
            : base(message)
        {
            Position = position;
            Length = length;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="token">Erroneous token</param>
        public ExpressionException(string message, IToken token)
            : base(message)
        {
            Position = token.Position;
            Length = token.Length;
        }
    }
}
