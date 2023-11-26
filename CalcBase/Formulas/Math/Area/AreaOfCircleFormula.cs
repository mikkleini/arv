using CalcBase.Constants.Mathematical;
using CalcBase.Numbers;
using CalcBase.Operators.Arithmetic;
using CalcBase.Quantities;
using CalcBase.Quantities.Mathematical;
using CalcBase.Quantities.Physical;

namespace CalcBase.Formulas.Math.Areas
{
    /// <summary>
    /// Area of circle
    /// </summary>
    public record AreaOfCircleFormula : Singleton<AreaOfCircleFormula>, IFormula
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name => "Area of circle";

        /// <summary>
        /// Formula as postfix expression
        /// </summary>
        public IElement[] Expression => new IElement[]
        {
            PiConstant.Instance,
            LengthQuantity.Instance,
            Number.Create(2),
            ExponentOperator.Instance,
            MultiplicationOperator.Instance,
        };

        /// <summary>
        /// Result of the formula
        /// </summary>
        public IQuantity Result => AreaQuantity.Instance;
    }
}

