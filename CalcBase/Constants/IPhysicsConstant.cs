using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcBase.Constants
{
    /// <summary>
    /// Interface for physics constant
    /// </summary>
    interface IPhysicsConstant : IRealConstant
    {
        string Unit { get; }
    }
}
