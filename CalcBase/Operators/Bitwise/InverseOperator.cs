using System.Numerics;

namespace CalcBase.Operators.Bitwise
{
    /// <summary>
    /// Inverse operator
    /// </summary>
    public record InverseOperator : IUnaryOperator
    {
        public string Symbol { get; } = "~";
        public string Name { get; } = "Bitwise NOT";
        public int Precedence { get; } = 3;
        public OperatorAssociativity Associativity { get; } = OperatorAssociativity.Right;
        public OperatorOpCountType OpCount { get; } = OperatorOpCountType.Unary;

        /// <summary>
        /// Calculate result of operation
        /// </summary>
        /// <param name="a">Operand A</param>
        /// <returns>Result of operation</returns>
        public NumberType Calculate(NumberType a)
        {
            if (!a.IsNatural())
            {
                throw new SolverException($"{Name} can only be performed with natural numbers");
            }

            return new NumberType(~((BigInteger)a));
        }
    }
}
