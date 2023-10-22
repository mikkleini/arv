using CalcBase.Tokens;

namespace CalcBase.Numbers
{
    /// <summary>
    /// Real number
    /// </summary>
    public record RealNumber : INumber
    {
        /// <summary>
        /// Value
        /// </summary>
        public required RealType Value { get; init; }

        /// <summary>
        /// Is presented with scientific notation ?
        /// </summary>
        public bool IsScientificNotation { get; init; } = false;

        /// <summary>
        /// Creator
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="isScientificNotation">Is scientific notation ?</param>
        public static RealNumber Create(RealType value, bool isScientificNotation = false)
        {
            return new RealNumber()
            {
                Value = value,
                IsScientificNotation = isScientificNotation
            };
        }
    }
}
