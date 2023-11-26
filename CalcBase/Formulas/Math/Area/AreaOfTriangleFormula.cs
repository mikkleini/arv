using CalcBase.Constants.Mathematical;
using CalcBase.Numbers;
using CalcBase.Operators.Arithmetic;
using CalcBase.Quantities;
using CalcBase.Quantities.Mathematical;
using CalcBase.Quantities.Physical;

namespace CalcBase.Formulas.Math.Areas
{
    /// <summary>
    /// Area of triangle
    /// </summary>
    public record AreaOfTriangleFormula : Singleton<AreaOfTriangleFormula>, IFormula
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name => "Area of triangle";

        /// <summary>
        /// Formula as postfix expression
        /// </summary>
        public IElement[] Expression => new IElement[]
        {
            LengthQuantity.Instance,
            LengthQuantity.Instance,
            MultiplicationOperator.Instance,
            Number.Create(2),
            DivisionOperator.Instance
        };

        /// <summary>
        /// Result of the formula
        /// </summary>
        public IQuantity Result => AreaQuantity.Instance;
    }
}

