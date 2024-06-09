using ArvBase.Quantities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArvBase.Units
{
    /// <summary>
    /// Non-SI unit that is accepted to be used
    /// </summary>
    public record NonSIUnit : IUnit
    {
        public UnitMultiple NominalMultiple => Multiples.Single(m => m.Factor == 1);
        public string Name => NominalMultiple.Name;
        public string[] Symbols => NominalMultiple.Symbols;
        public NumberType EqualSIValue { get; init; }
        public ISIUnit EqualSIUnit { get; init; }
        public IQuantity Quantity => EqualSIUnit.Quantity;
        public UnitMultiple[] Multiples { get; init; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="symbols">Symbol(s)</param>
        /// <param name="equalSIValue">Equal SI value</param>
        /// <param name="equalIUnit">Equal SI unit</param>
        /// <param name="multiples">Unit multiples</param>
        public NonSIUnit(NumberType equalSIValue, ISIUnit equalIUnit, UnitMultiple[] multiples)
        {
            EqualSIValue = equalSIValue;
            EqualSIUnit = equalIUnit;
            Multiples = multiples.Select(m => m with { Parent = this }).ToArray();
        }
    }
}
