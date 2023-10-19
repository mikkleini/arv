using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcBase
{
    public enum TokenType
    {
        BinaryNumber,
        IntegerNumber,
        HexadecimalNumber,
        RealNumber,
        Text,
        Unit,
        Variable,
        Function,
        Parenthesis,
        Operator,
        Comma
    }
}
