using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcBase.Operators.Bitwise
{
    public record XorOperator : IOperator
    {
        public string Symbol => "^";
        public string Name => "Bitwise XOR";
        public int Precedence => 12;
        public OperatorAssociativity Associativity => OperatorAssociativity.Left;
        public bool IsUnary => false;
    }
}
