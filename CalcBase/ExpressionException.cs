namespace CalcBase
{
    /// <summary>
    /// Expression exception
    /// </summary>
    internal class ExpressionException : Exception
    {
        /// <summary>
        /// Errornous expresison position
        /// </summary>
        public int Position { get; init; }

        /// <summary>
        /// Errornous expression length
        /// </summary>
        public int Length { get; init; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="position">Errornous expression position</param>
        /// <param name="length">Errornous expression length</param>
        public ExpressionException(string message, int position, int length)
            : base(message)
        {
            Position = position;
            Length = length;
        }
    }
}
