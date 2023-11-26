using CalcBase.Constants.Mathematical;
using CalcBase.Numbers;
using CalcBase.Operators.Arithmetic;
using CalcBase.Quantities;
using CalcBase.Quantities.Mathematical;
using CalcBase.Quantities.Physical;

namespace CalcBase.Formulas.Math.Areas
{
    /// <summary>
    /// Area of sphere
    /// </summary>
    public record AreaOfSphereFormula : Singleton<AreaOfCircleFormula>, IFormula
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name => "Area of sphere";

        /// <summary>
        /// Formula as postfix expression
        /// </summary>
        public IElement[] Expression => new IElement[]
        {
            Number.Create(4),
            PiConstant.Instance,
            LengthQuantity.Instance,
            Number.Create(2),
            ExponentOperator.Instance,
            MultiplicationOperator.Instance,
            MultiplicationOperator.Instance,
        };

        /// <summary>
        /// Result of the formula
        /// </summary>
        public IQuantity Result => AreaQuantity.Instance;
    }
}

