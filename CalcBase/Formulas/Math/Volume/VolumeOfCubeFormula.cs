using CalcBase.Constants.Mathematical;
using CalcBase.Numbers;
using CalcBase.Operators.Arithmetic;
using CalcBase.Quantities;
using CalcBase.Quantities.Mathematical;
using CalcBase.Quantities.Physical;

namespace CalcBase.Formulas.Math.Areas
{
    /// <summary>
    /// Volume of cube
    /// </summary>
    public record VolumeOfCubeFormula : Singleton<VolumeOfCubeFormula>, IFormula
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name => "Volume of cube";

        /// <summary>
        /// Formula as postfix expression
        /// </summary>
        public IElement[] Expression => new IElement[]
        {
            LengthQuantity.Instance,
            Number.Create(3),
            ExponentOperator.Instance
        };

        /// <summary>
        /// Result of the formula
        /// </summary>
        public IQuantity Result => VolumeQuantity.Instance;
    }
}

