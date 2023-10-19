﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcBase.Constants
{
    /// <summary>
    /// Interface for real number constant
    /// </summary>
    interface IRealConstant : IConstant
    {
        RealType Value { get; }
    }
}
