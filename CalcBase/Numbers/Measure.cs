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
        public IUnit Unit { get; init; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="unit">Unit</param>
        /// <param name="radix">Radix</param>
        /// <param name="isScientificNotation">Is scientific notation ?</param>
        /// <param name="dominantCase">Dominant hexadecimal number case</param>
        public Measure(NumberType value, IUnit unit, IntegerRadix radix = IntegerRadix.Decimal,
            bool isScientificNotation = false, DominantHexadecimalCase dominantCase = DominantHexadecimalCase.None)
            : base(value, radix, isScientificNotation, dominantCase)
        {
            Unit = unit;
        }

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
            return new Measure(value, unit, radix, isScientificNotation, dominantCase);
        }
    }
}
