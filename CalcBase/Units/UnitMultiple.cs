using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcBase.Units
{
    /// <summary>
    /// Unit multiple
    /// </summary>
    /// <param name="Name">Name</param>
    /// <param name="Symbols">Symbols</param>
    /// <param name="Factor">Factor</param>
    /// <param name="Context">Context</param>
    public record struct UnitMultiple(string Name, string[] Symbols, NumberType Factor, UnitContext Context);
}
