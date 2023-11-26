using System.Numerics;

namespace CalcBase.Operators.Bitwise
{
    /// <summary>
    /// Bitwise XOR operator
    /// </summary>
    public record XorOperator : IBinaryOperator
    {
        public string Symbol => "^";
        public string Name => "Bitwise XOR";
        public int Precedence => 12;
        public OperatorAssociativity Associativity => OperatorAssociativity.Left;
        public OperatorOpCountType OpCount => OperatorOpCountType.Binary;

        /// <summary>
        /// Calculate result of operation
        /// </summary>
        /// <param name="a">Operand A</param>
        /// <param name="b">Operand B</param>
        /// <returns>Result of operation</returns>
        public NumberType Calculate(NumberType a, NumberType b)
        {
            if ((NumberType.Sign(a) < 0) || !NumberType.IsInteger(a))
            {
                throw new SolverException($"{Name} can only be performed with natural numbers");
            }

            if ((NumberType.Sign(b) < 0) || !NumberType.IsInteger(b))
            {
                throw new SolverException($"{Name} can only be performed with natural numbers");
            }

            var bigA = (BigInteger)a;
            var bigB = (BigInteger)b;
            return new NumberType(bigA ^ bigB);
        }
    }
}
