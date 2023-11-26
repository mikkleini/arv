using CalcBase.Constants.Mathematical;
using CalcBase.Numbers;
using CalcBase.Operators.Arithmetic;
using CalcBase.Quantities;
using CalcBase.Quantities.Mathematical;
using CalcBase.Quantities.Physical;

namespace CalcBase.Formulas.Math.Areas
{
    /// <summary>
    /// Volume of sphere
    /// </summary>
    public record VolumeOfSphereFormula : Singleton<VolumeOfSphereFormula>, IFormula
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name => "Volume of sphere";

        /// <summary>
        /// Formula as postfix expression
        /// </summary>
        public IElement[] Expression => new IElement[]
        {
            Number.Create(4),
            Number.Create(3),
            DivisionOperator.Instance,
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
        public IQuantity Result => VolumeQuantity.Instance;
    }
}

