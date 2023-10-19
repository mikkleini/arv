using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcBase.Operators.Arithmetic
{
    public record SubtractionOperator : IOperator
    {
        public string Symbol => "-";
        public string Name => "Subtract";
        public int Precedence => 6;
        public OperatorAssociativity Associativity => OperatorAssociativity.Left;
        public bool IsUnary => false;
    }
}
