using CalcBase.Quantities;
using CalcBase.Units;

namespace CalcBase.Formulas
{
    /// <summary>
    /// Formula
    /// </summary>
    public interface IFormula : IElement
    {
        /// <summary>
        /// Name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Formula as postfix expression
        /// </summary>
        IElement[] Expression { get; }

        /// <summary>
        /// Result of the formula
        /// </summary>
        IQuantity Result { get; }
    }
}
