using System.Numerics;

namespace CalcBase.Operators.Arithmetic
{
    /// <summary>
    /// Quotient operator
    /// </summary>
    public record QuotientOperator : IBinaryOperator
    {
        public string Symbol { get; } = "//";
        public string Name { get; } = "Quotient";
        public int Precedence { get; } = 4;
        public OperatorAssociativity Associativity { get; } = OperatorAssociativity.Left;
        public OperatorOpCountType OpCount { get; } = OperatorOpCountType.Binary;

        /// <summary>
        /// Calculate result of operation
        /// </summary>
        /// <param name="a">Operand A</param>
        /// <param name="b">Operand B</param>
        /// <returns>Result of operation</returns>
        public NumberType Calculate(NumberType a, NumberType b)
        {
            return NumberType.Floor(a / b);
        }
    }
}
