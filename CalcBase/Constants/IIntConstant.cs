using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcBase.Constants
{
    /// <summary>
    /// Interface for integer number constant
    /// </summary>
    interface IIntConstant
    {
        string Name { get; }
        string Symbol { get; }
        IntType Value { get; }
    }
}
