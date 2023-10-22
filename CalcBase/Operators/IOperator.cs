﻿namespace CalcBase.Operators
{
    public interface IOperator
    {
        string Symbol { get; }
        string Name { get; }
        int Precedence { get; }
        OperatorAssociativity Associativity { get; }
        bool IsUnary { get; }
    }
}
