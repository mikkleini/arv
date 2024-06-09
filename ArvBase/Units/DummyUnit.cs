using ArvBase.Quantities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArvBase.Units
{
    internal class DummyUnit : IUnit
    {
        public string Name => "Dummy";
        public string[] Symbols => ["d"];
        public IQuantity Quantity => new Quantity("Dummy", ["d"]);
        public UnitMultiple[] Multiples => [];
        public UnitMultiple NominalMultiple => new("dummy", ["d"], 1);
    }
}
