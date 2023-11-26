using CalcBase.Operators.Arithmetic;
using CalcBase.Quantities;
using CalcBase.Quantities.Physical;

namespace CalcBase.Formulas.Physics
{
    /// <summary>
    /// Velocity formula
    /// </summary>
    public record VelocityFormula : Singleton<VelocityFormula>, IFormula
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name => "Velocity";

        /// <summary>
        /// Formula as postfix expression
        /// </summary>
        public IElement[] Expression => new IElement[]
        {
            LengthQuantity.Instance,
            TimeQuantity.Instance,
            DivisionOperator.Instance,
        };

        /// <summary>
        /// Result of the formula
        /// </summary>
        public IQuantity Result => SpeedQuantity.Instance;
    }
}
