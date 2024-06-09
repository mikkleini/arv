using ArvBase.Numbers;
using ArvBase.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArvBase
{
    public static class MeasureExtension
    {
        /// <summary>
        /// Get measure nominal SI value
        /// </summary>
        /// <param name="measure">Measure</param>
        /// <returns>Nominal SI value</returns>
        public static NumberType GetNominalSIValue(this Measure measure)
        {
            if (measure.Unit.Parent is ISIUnit)
            {
                return measure.Value * measure.Unit.Factor;
            }
            else if (measure.Unit.Parent is NonSIUnit nonSIUnit)
            {
                return measure.Value * measure.Unit.Factor * nonSIUnit.EqualSIValue;
            }
            else if (measure.Unit.Parent is ImperialUnit imperialUnit)
            {
                return measure.Value * measure.Unit.Factor * imperialUnit.EqualSIValue;
            }

            throw new Exception($"Measure {measure} has no base SI unit");
        }

        /// <summary>
        /// Get measure base SI unit
        /// </summary>
        /// <param name="measure">Measure</param>
        /// <returns>Base SI unit</returns>
        public static ISIUnit GetBaseSIUnit(this Measure measure)
        {
            if (measure.Unit.Parent is ISIUnit siUnit)
            {
                return siUnit;
            }
            else if (measure.Unit.Parent is NonSIUnit nonSIUnit)
            {
                return nonSIUnit.EqualSIUnit;
            }
            else if (measure.Unit.Parent is ImperialUnit imperialUnit)
            {
                return imperialUnit.EqualSIUnit;
            }

            throw new Exception($"Measure {measure} has no base SI unit");
        }
    }
}
