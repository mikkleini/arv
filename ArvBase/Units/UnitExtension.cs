using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArvBase.Units
{
    public static class UnitExtension
    {
        /// <summary>
        /// Unit multiple by name
        /// </summary>
        /// <param name="unit">Unit</param>
        /// <param name="multipleName">Multiple name</param>
        /// <returns>Unit multiple</returns>
        public static UnitMultiple MultipleNamed(this IUnit unit, string multipleName)
        {
            return unit.Multiples.Single(x => x.Name == multipleName);
        }
    }
}
