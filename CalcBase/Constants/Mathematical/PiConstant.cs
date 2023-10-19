using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcBase.Constants.Mathematical
{
    public class PiConstant : IRealConstant
    {
        public string Name => "Pi";
        public string Symbol => "π";
        public RealType Value => 3.14159265358979323846m;
    }
}
