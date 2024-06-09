namespace ArvBase.Functions
{
    /// <summary>
    /// Single argument function
    /// </summary>
    public record SingleArgumentFunction : ISingleArgumentFunction
    {
        private readonly Func<NumberType, NumberType> function;

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
        public SingleArgumentFunction(string name, string symbol, Func<NumberType, NumberType> function)
        {
            Name = name;
            Symbol = symbol;
            this.function = function;
        }

        /// <summary>
        /// Calculate function value
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Value</returns>
        public NumberType Calculate(NumberType x)
        {
            return function(x);
        }
    }
}
