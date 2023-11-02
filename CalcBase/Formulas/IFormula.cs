using CalcBase.Tokens;
using CalcBase.Units;

namespace CalcBase.Formulas
{
    public interface IFormula
    {
        /// <summary>
        /// Formula as postfix expression
        /// </summary>
        IToken[] Expression { get; }

        /// <summary>
        /// Result of the formula
        /// </summary>
        IUnit Result { get; }
    }
}
