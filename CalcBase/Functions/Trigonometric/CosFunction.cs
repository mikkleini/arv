namespace CalcBase.Functions.Trigonometric
{
    public record CosFunction : Singleton<CosFunction>, ISingleArgumentFunction
    {
        public string Name => "Cosine";
        public string Symbol => "cos";

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
