using DecimalMath;

namespace CalcBase.Functions
{
    public record CosFunction : ISingleArgumentRealFunction
    {
        public string Name => "Cosine";
        public string Symbol => "cos";

        /// <summary>
        /// Calculate cosine function value
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Cosine value</returns>
        public RealType Calculate(RealType x)
        {
            return DecimalEx.Cos(x);
        }
    }
}
