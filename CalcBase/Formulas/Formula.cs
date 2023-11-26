using CalcBase.Quantities;

namespace CalcBase.Formulas
{
    /// <summary>
    /// Formula
    /// </summary>
    public record Formula : IFormula
    {
        public string Name { get; init; }
        public IElement[] Expression { get; init; }
        public IQuantity Result { get; init; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="expression">Expression</param>
        /// <param name="result">Result</param>
        public Formula(string name, IElement[] expression, IQuantity result)
        {
            Name = name;
            Expression = expression;
            Result = result;
        }
    }
}
