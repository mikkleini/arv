namespace CalcBase
{
    public record ExpressionError
    {
        /// <summary>
        /// Error message
        /// </summary>
        public required string Message { get; init; }

        /// <summary>
        /// Errornous expresison position
        /// </summary>
        public int Position { get; init; }

        /// <summary>
        /// Errornous expression length
        /// </summary>
        public int Length { get; init; }
    }
}
