using CalcBase.Units;

namespace CalcBase.Numbers
{
    /// <summary>
    /// Number
    /// </summary>
    public record Number : IElement
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
        public DominantHexadecimalCase DominantCase { get; init; } = DominantHexadecimalCase.None;

        /// <summary>
        /// Is presented with scientific notation ?
        /// </summary>
        public bool IsScientificNotation { get; init; } = false;

        /// <summary>
        /// Creator
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="radix">Radix</param>
        /// <param name="isScientificNotation">Is scientific notation ?</param>
        /// <param name="dominantCase">Dominant hexadecimal number case</param>
        public static Number Create(NumberType value, IntegerRadix radix = IntegerRadix.Decimal,
            bool isScientificNotation = false, DominantHexadecimalCase dominantCase = DominantHexadecimalCase.None)
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
