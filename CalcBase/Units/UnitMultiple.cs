using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcBase.Units
{
    /// <summary>
    /// (SI) unit multiple
    /// </summary>
    /// <param name="symbol">Symbol</param>
    /// <param name="name">Name</param>
    /// <param name="factor">Factor</param>
    public record struct UnitMultiple(string symbol, string name, NumberType factor);
}
