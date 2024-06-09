using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArvBase.Units
{
    /// <summary>
    /// Unit context flags
    /// </summary>
    [Flags]
    public enum UnitContext
    {
        All = 0xFF,
        ElectricalEngineering = 2,
        Programming = 4,
        Nautical = 8,
        Engineering = 16,
        Astronomy = 32,
    }
}
