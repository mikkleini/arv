using CalcBase.Units;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public NumberType Value { get; init; }

        /// <summary>
        /// Presentation radix
        /// </summary>
        public IntegerRadix Radix { get; init; }

        /// <summary>
        /// In case of hexadecimal radix - dominant letter case
        /// </summary>
        public DominantHexadecimalCase DominantCase { get; init; } = DominantHexadecimalCase.None;

        /// <summary>
        /// Is presented with scientific notation ?
        /// </summary>
        public bool IsScientificNotation { get; init; } = false;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="radix">Radix</param>
        /// <param name="isScientificNotation">Is scientific notation ?</param>
        /// <param name="dominantCase">Dominant hexadecimal number case</param>
        public Number(NumberType value, IntegerRadix radix = IntegerRadix.Decimal,
            bool isScientificNotation = false, DominantHexadecimalCase dominantCase = DominantHexadecimalCase.None)
        {
            Value = value;
            Radix = radix;
            DominantCase = dominantCase;
            IsScientificNotation = isScientificNotation;
        }

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
            return new Number(value, radix, isScientificNotation, dominantCase);
        }

        /// <summary>
        /// Implicit operator for NumberType
        /// </summary>
        /// <param name="n">Number type</param>
        public static implicit operator Number(NumberType n) => new(n);

        /// <summary>
        /// Implicit operator for decimal type
        /// </summary>
        /// <param name="d">Decimal</param>
        public static implicit operator Number(decimal d) => new((NumberType)d);

        /// <summary>
        /// Implicit operator for integer type
        /// </summary>
        /// <param name="i">Integer</param>
        public static implicit operator Number(int i) => new((NumberType)i);
    }
}
