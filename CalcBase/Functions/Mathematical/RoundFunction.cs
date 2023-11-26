using CalcBase.Functions.Trigonometric;

namespace CalcBase.Functions.Mathematical
{
    /// <summary>
    /// Rounding function
    /// </summary>
    public record RoundFunction : IDualArgumentFunction
    {
        public string Name { get; } = "Round";
        public string Symbol { get; } = "round";

        /// <summary>
        /// Calculate function
        /// </summary
        /// <param name="a">Argument A</param>
        /// <param name="b">Argument B</param>
        /// <returns></returns>
        public NumberType Calculate(NumberType a, NumberType b)
        {
            return NumberType.Round(a, (int)b);
        }
    }
}
