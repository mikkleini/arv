namespace ArvBase.Functions
{
    /// <summary>
    /// Dual argument function
    /// </summary>
    public record DualArgumentFunction : IDualArgumentFunction
    {
        private readonly Func<NumberType, NumberType, NumberType> function;

        /// <summary>
        /// Name of the function
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Symbol of the function
        /// </summary>
        public string Symbol { get; init; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="symbol">Symbol</param>
        /// <param name="function">Function</param>
        public DualArgumentFunction(string name, string symbol, Func<NumberType, NumberType, NumberType> function)
        {
            Name = name;
            Symbol = symbol;
            this.function = function;
        }

        /// <summary>
        /// Calculate function value
        /// </summary>
        /// <param name="a">Argument 1</param>
        /// <param name="b">Argument 2</param>
        /// <returns>Value</returns>
        public NumberType Calculate(NumberType a, NumberType b)
        {
            return function(a, b);
        }
    }
}
