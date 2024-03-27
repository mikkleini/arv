using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcBase.Units
{
    public interface IDerivedUnit
    {
        IElement[] Expression { get; }
    }
}
