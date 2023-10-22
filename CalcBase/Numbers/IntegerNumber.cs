using CalcBase.Tokens;

namespace CalcBase.Numbers
{
    /// <summary>
    /// Integer number token
    /// </summary>
    public record IntegerNumber : INumber
    {
        /// <summary>
        /// Value
        /// </summary>
        public required IntType Value { get; init; }

        /// <summary>
        /// Presentation radix
        /// </summary>
        public required IntegerRadix Radix { get; init; }

        /// <summary>
        /// In case of hexadecimal radix - dominant letter case
        /// </summary>
        public DominantCase DominantCase { get; init; } = DominantCase.None;

        /// <summary>
        /// Is presented with scientific notation ?
        /// </summary>
        public bool IsScientificNotation { get; init; } = false;

        /// <summary>
        /// Creator
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="radix">Radix</param>
        /// <param name="dominantCase">Dominant hexadecimal number case</param>
        /// <param name="isScientificNotation">Is scientific notation ?</param>
        public static IntegerNumber Create(IntType value, IntegerRadix radix = IntegerRadix.Decimal, DominantCase dominantCase = DominantCase.None, bool isScientificNotation = false)
        {
            return new IntegerNumber()
            {
                Value = value,
                Radix = radix,
                DominantCase = dominantCase,
                IsScientificNotation = isScientificNotation
            };
        }
    }
}
