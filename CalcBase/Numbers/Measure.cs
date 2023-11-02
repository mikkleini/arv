using CalcBase.Units;

namespace CalcBase.Numbers
{
    /// <summary>
    /// Measure
    /// </summary>
    public record Measure : Number
    {
        /// <summary>
        /// Unit
        /// </summary>
        public required IUnit Unit { get; init; }

        /// <summary>
        /// Creator
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="unit">Unit</param>
        /// <param name="radix">Radix</param>
        /// <param name="isScientificNotation">Is scientific notation ?</param>
        /// <param name="dominantCase">Dominant hexadecimal number case</param>
        public static Measure Create(NumberType value, IUnit unit, IntegerRadix radix = IntegerRadix.Decimal,
            bool isScientificNotation = false, DominantHexadecimalCase dominantCase = DominantHexadecimalCase.None)
        {
            return new Measure()
            {
                Value = value,
                Unit = unit,
                Radix = radix,
                DominantCase = dominantCase,
                IsScientificNotation = isScientificNotation
            };
        }
    }
}
