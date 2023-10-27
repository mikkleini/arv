using CalcBase.Tokens;

namespace CalcBase.Numbers
{
    /// <summary>
    /// Number
    /// </summary>
    public record Number
    {
        /// <summary>
        /// Value
        /// </summary>
        public required NumberType Value { get; init; }

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
        public static Number Create(NumberType value, IntegerRadix radix = IntegerRadix.Decimal, DominantCase dominantCase = DominantCase.None, bool isScientificNotation = false)
        {
            return new Number()
            {
                Value = value,
                Radix = radix,
                DominantCase = dominantCase,
                IsScientificNotation = isScientificNotation
            };
        }
    }
}
