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
        /// Result unit of the formula
        /// </summary>
        ISIUnit ResultUnit { get; }
    }
}
