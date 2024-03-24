using CalcBase.Quantities;
using CalcBase.Units;

namespace CalcBase.Formulas
{
    /// <summary>
    /// Formula
    /// </summary>
    public record Formula : IFormula
    {
        public string Name { get; init; }
        public IElement[] Expression { get; init; }
        public ISIUnit ResultUnit { get; init; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="expression">Expression</param>
        /// <param name="resultUnit">Resulting SI unit</param>
        public Formula(string name, IElement[] expression, ISIUnit resultUnit)
        {
            Name = name;
            Expression = expression;
            ResultUnit = resultUnit;
        }
    }
}
