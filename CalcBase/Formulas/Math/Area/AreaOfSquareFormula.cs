using CalcBase.Constants.Mathematical;
using CalcBase.Numbers;
using CalcBase.Operators.Arithmetic;
using CalcBase.Quantities;
using CalcBase.Quantities.Mathematical;
using CalcBase.Quantities.Physical;

namespace CalcBase.Formulas.Math.Areas
{
    /// <summary>
    /// Area of square
    /// </summary>
    public record AreaOfSquareFormula : Singleton<AreaOfSquareFormula>, IFormula
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name => "Area of square";

        /// <summary>
        /// Formula as postfix expression
        /// </summary>
        public IElement[] Expression => new IElement[]
        {
            LengthQuantity.Instance,
            Number.Create(2),
            ExponentOperator.Instance
        };

        /// <summary>
        /// Result of the formula
        /// </summary>
        public IQuantity Result => AreaQuantity.Instance;
    }
}

