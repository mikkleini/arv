using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcBase.Operators.Conversion
{
    /// <summary>
    /// Unit conversion operator
    /// </summary>
    public record UnitConversionOperator : IBinaryOperator
    {
        public string Symbol { get; } = ">";
        public string Name { get; } = "Conversion";
        public int Precedence { get; } = 99;
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
            return a + b;
        }
    }
}
