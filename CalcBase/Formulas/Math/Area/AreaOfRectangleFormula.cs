using CalcBase.Constants.Mathematical;
using CalcBase.Numbers;
using CalcBase.Operators.Arithmetic;
using CalcBase.Quantities;
using CalcBase.Quantities.Mathematical;
using CalcBase.Quantities.Physical;

namespace CalcBase.Formulas.Math.Areas
{
    /// <summary>
    /// Area of rectangle
    /// </summary>
    public record AreaOfRectangleFormula : Singleton<AreaOfRectangleFormula>, IFormula
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name => "Area of rectangle";

        /// <summary>
        /// Formula as postfix expression
        /// </summary>
        public IElement[] Expression => new IElement[]
        {
            LengthQuantity.Instance,
            LengthQuantity.Instance,
            MultiplicationOperator.Instance,
        };

        /// <summary>
        /// Result of the formula
        /// </summary>
        public IQuantity Result => AreaQuantity.Instance;
    }
}

