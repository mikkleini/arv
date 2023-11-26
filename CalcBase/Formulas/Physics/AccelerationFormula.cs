using CalcBase.Operators.Arithmetic;
using CalcBase.Quantities;
using CalcBase.Quantities.Physical;

namespace CalcBase.Formulas.Physics
{
    /// <summary>
    /// Acceleration formula
    /// </summary>
    public record AccelerationFormula : Singleton<VelocityFormula>, IFormula
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name => "Acceleration";

        /// <summary>
        /// Formula as postfix expression
        /// </summary>
        public IElement[] Expression => new IElement[]
        {
            SpeedQuantity.Instance,
            TimeQuantity.Instance,
            DivisionOperator.Instance,
        };

        /// <summary>
        /// Result of the formula
        /// </summary>
        public IQuantity Result => SpeedQuantity.Instance;
    }
}

