namespace CalcBase.Functions.Trigonometric
{
    /// <summary>
    /// Cosine function
    /// </summary>
    public record CosFunction : ISingleArgumentFunction
    {
        public string Name { get; } = "Cosine";
        public string Symbol { get; } = "cos";

        /// <summary>
        /// Calculate cosine function value
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Cosine value</returns>
        public NumberType Calculate(NumberType x)
        {
            return NumberType.Cos(x);
        }
    }
}
