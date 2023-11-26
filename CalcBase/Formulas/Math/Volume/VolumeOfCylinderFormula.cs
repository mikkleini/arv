using CalcBase.Constants.Mathematical;
using CalcBase.Numbers;
using CalcBase.Operators.Arithmetic;
using CalcBase.Quantities;
using CalcBase.Quantities.Mathematical;
using CalcBase.Quantities.Physical;

namespace CalcBase.Formulas.Math.Areas
{
    /// <summary>
    /// Volume of cylinder
    /// </summary>
    public record VolumeOfCylinderFormula : Singleton<VolumeOfCylinderFormula>, IFormula
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name => "Volume of cylinder";

        /// <summary>
        /// Formula as postfix expression
        /// </summary>
        public IElement[] Expression => new IElement[]
        {
            LengthQuantity.Instance,
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

