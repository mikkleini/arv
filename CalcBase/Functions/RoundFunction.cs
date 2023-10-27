namespace CalcBase.Functions
{
    public record RoundFunction : IDualArgumentFunction
    {
        public string Name => "Round";
        public string Symbol => "round";

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
