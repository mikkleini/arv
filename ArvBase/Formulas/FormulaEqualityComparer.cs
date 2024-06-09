using ArvBase.Numbers;
using ArvBase.Units;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ArvBase.Formulas
{
    /// <summary>
    /// Formula equality comparer
    /// </summary>
    public class FormulaEqualityComparer : IEqualityComparer<IElement>
    {
        /// <summary>
        /// Get base SI unit
        /// </summary>
        /// <param name="anything"></param>
        /// <returns></returns>
        private ISIUnit? TryGetBaseSIUnit(IElement anything)
        {
            if (anything is ISIUnit siUnit)
            {
                return siUnit;
            }
            else if (anything is UnitMultiple multiple)
            {
                if (multiple.Parent is ISIUnit parentSI)
                {
                    return parentSI;
                }
                else if (multiple.Parent is NonSIUnit nonSIUnit)
                {
                    return nonSIUnit.EqualSIUnit;
                }
                else if (multiple.Parent is ImperialUnit imperialUnit)
                {
                    return imperialUnit.EqualSIUnit;
                }
                else
                {
                    throw new NotImplementedException($"Unexpected unit multiple parent type: {multiple.Parent}");
                }
            }
            else if (anything is PhysicsVariable phyVar)
            {
                if (phyVar.Unit is ISIUnit phySIUnit)
                {
                    return phySIUnit;
                }
                else if (phyVar.Unit is NonSIUnit phyNonSIUnit)
                {
                    return phyNonSIUnit.EqualSIUnit;
                }
                else if (phyVar.Unit is ImperialUnit phyImperialUnit)
                {
                    return phyImperialUnit.EqualSIUnit;
                }
                else
                {
                    throw new NotImplementedException($"Unexpected physical variable unit type: {phyVar.Unit}");
                }
            }
            else if (anything is Measure measure)
            {
                if (measure.Unit.Parent is ISIUnit parentSI)
                {
                    return parentSI;
                }
                else if (measure.Unit.Parent is NonSIUnit nonSIUnit)
                {
                    return nonSIUnit.EqualSIUnit;
                }
                else if (measure.Unit.Parent is ImperialUnit imperialUnit)
                {
                    return imperialUnit.EqualSIUnit;
                }
                else
                {
                    throw new NotImplementedException($"Unexpected unit multiple parent type: {measure.Unit.Parent}");
                }
            }
            else
            {
                return null;
            }
        }

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

            if (x == null || y == null)
            {
                return false;
            }

            if ((x is Number numberX) && (y is Number numberY) && (numberX.Value == numberY.Value))
            {
                return true;
            }

            ISIUnit? unitA = TryGetBaseSIUnit(x);
            ISIUnit? unitB = TryGetBaseSIUnit(y);

            return unitA?.Equals(unitB) ?? false;
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
