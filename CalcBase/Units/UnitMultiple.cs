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
    /// <param name="Symbol">Symbol</param>
    /// <param name="Name">Name</param>
    /// <param name="Factor">Factor</param>
    public record struct UnitMultiple(string Symbol, string Name, NumberType Factor);
}
