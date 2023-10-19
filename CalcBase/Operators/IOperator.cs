using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcBase.Operators
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
