using System.Numerics;

namespace CalcBase.Operators.Arithmetic
{
    /// <summary>
    /// Exponent operator
    /// </summary>
    public record ExponentOperator : IBinaryOperator
    {
        public string Symbol { get; } = "**";
        public string Name { get; } = "Exponent";
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
            return NumberType.Pow(a, b);
        }
    }
}
