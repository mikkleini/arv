using CalcBase.Generic;
using CalcBase.Tokens;
using DecimalMath;
using System.Numerics;

namespace CalcBase.Operators.Arithmetic
{
    /// <summary>
    /// Exponent operator
    /// </summary>
    public record ExponentOperator : IOperator, IBinaryIntegerOperation, IBinaryRealOperation
    {
        public string Symbol => "**";
        public string Name => "Exponent";
        public int Precedence => 4;
        public OperatorAssociativity Associativity => OperatorAssociativity.Left;
        public OperatorOpCountType OpCount => OperatorOpCountType.Binary;

        /// <summary>
        /// Calculate result of operation
        /// </summary>
        /// <param name="a">Operand A</param>
        /// <param name="b">Operand B</param>
        /// <returns>Result of operation</returns>
        public IntType Calculate(IntType a, IntType b)
        {
            // TODO To some error checking?
            int ib = (int)b;            
            return BigInteger.Pow(a, ib);
        }

        /// <summary>
        /// Calculate result of operation
        /// </summary>
        /// <param name="a">Operand A</param>
        /// <param name="b">Operand B</param>
        /// <returns>Result of operation</returns>
        public RealType Calculate(RealType a, RealType b)
        {
            return DecimalEx.Pow(a, b);
        }
    }
}
