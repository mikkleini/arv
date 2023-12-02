﻿using System.Numerics;
using System.Runtime.CompilerServices;

namespace CalcBase
{
    public static class NumberExtension
    {
        /// <summary>
        /// Is BigRational an integer ?
        /// </summary>
        /// <param name="rational">Rational</param>
        /// <returns>true if integer, false if not</returns>
        public static bool IsInteger(this BigRational rational)
        {
            return BigRational.IsInteger(rational);
        }

        /// <summary>
        /// Is BigRational a natural number ?
        /// </summary>
        /// <param name="rational">Rational</param>
        /// <returns>true if natural, false if not</returns>
        public static bool IsNatural(this BigRational rational)
        {
            return BigRational.IsInteger(rational) && (rational >= 0);
        }
    }
}
