using CalcBase.Constants.Mathematical;
using CalcBase.Numbers;
using CalcBase.Operators.Arithmetic;
using CalcBase.Quantities;
using CalcBase.Quantities.Mathematical;
using CalcBase.Quantities.Physical;

namespace CalcBase.Formulas.Math.Areas
{
    /// <summary>
    /// Volume of box (rectangular cuboid)
    /// </summary>
    public record VolumeOfBoxFormula : Singleton<VolumeOfBoxFormula>, IFormula
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name => "Volume of box (rectangular cuboid)";

        /// <summary>
        /// Formula as postfix expression
        /// </summary>
        public IElement[] Expression => new IElement[]
        {
            LengthQuantity.Instance,
            LengthQuantity.Instance,
            LengthQuantity.Instance,
            MultiplicationOperator.Instance,
            MultiplicationOperator.Instance,
        };

        /// <summary>
        /// Result of the formula
        /// </summary>
        public IQuantity Result => VolumeQuantity.Instance;
    }
}

