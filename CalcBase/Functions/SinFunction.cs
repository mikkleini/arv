using DecimalMath;

namespace CalcBase.Functions
{
    public record SinFunction : ISingleArgumentRealFunction
    {
        public string Name => "Sine";
        public string Symbol => "sin";

        /// <summary>
        /// Calculate sine function value
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Sine value</returns>
        public RealType Calculate(RealType x)
        {
            return DecimalEx.Sin(x);
        }
    }
}
