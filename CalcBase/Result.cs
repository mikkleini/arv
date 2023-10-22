namespace CalcBase
{
    public record Result<T, E>        
    {
        /// <summary>
        /// Value
        /// </summary>
        public T? Value { get; init; }

        /// <summary>
        /// Error
        /// </summary>
        public E? Error { get; init; }
    }
}
