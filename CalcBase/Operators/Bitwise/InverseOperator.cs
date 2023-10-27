using CalcBase.Generic;
using System.Numerics;

namespace CalcBase.Operators.Bitwise
{
    /// <summary>
    /// Inverse operator
    /// </summary>
    public record InverseOperator : IOperator, IUnaryOperation
    {
        public string Symbol => "~";
        public string Name => "Bitwise NOT";
        public int Precedence => 3;
        public OperatorAssociativity Associativity => OperatorAssociativity.Right;
        public OperatorOpCountType OpCount => OperatorOpCountType.Unary;

        /// <summary>
        /// Calculate result of operation
        /// </summary>
        /// <param name="a">Operand A</param>
        /// <returns>Result of operation</returns>
        public NumberType Calculate(NumberType a)
        {
            if ((NumberType.Sign(a) < 0) || !NumberType.IsInteger(a))
            {
                throw new SolverException($"{Name} can only be performed with natural number");
            }            

            var bigA = (BigInteger)a;
            return new NumberType(~bigA);
        }
    }
}
