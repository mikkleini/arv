using CalcBase.Units;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcBase.Formulas
{
    /// <summary>
    /// Formula equality comparer
    /// </summary>
    public class FormulaEqualityComparer : IEqualityComparer<IElement>
    {
        /// <summary>
        /// Check equality of two elements in array
        /// </summary>
        /// <param name="x">Element X</param>
        /// <param name="y">Element Y</param>
        /// <returns>true if are equal</returns>
        public bool Equals(IElement? x, IElement? y)
        {
            if (x == y)
            {
                return true;
            }

            IElement? realX = x;
            IElement? realY = y;

            if (x is IUnit unitX)
            {
                realX = unitX.Quantity;
            }
            else if (x is PhysicsVariable varX)
            {
                realX = varX.Quantity;
            }

            if (y is IUnit unitY)
            {
                realY = unitY.Quantity;
            }
            else if (y is PhysicsVariable phyVar)
            {
                realY = phyVar.Quantity;
            }

            return realX?.Equals(realY) ?? false;
        }

        /// <summary>
        /// Get has code of element
        /// </summary>
        /// <param name="obj">Element</param>
        /// <returns>Hash code</returns>
        public int GetHashCode([DisallowNull] IElement obj)
        {
            return obj.GetHashCode();
        }
    }
}
