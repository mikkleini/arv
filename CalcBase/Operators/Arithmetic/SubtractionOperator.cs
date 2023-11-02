﻿namespace CalcBase.Operators.Arithmetic
{
    /// <summary>
    /// Subtraction operator
    /// </summary>
    public record SubtractionOperator : Singleton<SubtractionOperator>, IBinaryOperator
    {
        public string Symbol => "-";
        public string Name => "Subtract";
        public int Precedence => 6;
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
            return a - b;
        }
    }
}
