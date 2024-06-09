namespace ArvBase.Functions.Mathematical
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
            if (!b.IsInteger())
            {
                throw new Exception("Digits argument is not integer");
            }

            return NumberType.Round(a, (int)b);
        }
    }
}
