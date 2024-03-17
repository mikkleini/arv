﻿using CalcBase.Quantities;
using System.Diagnostics.CodeAnalysis;

namespace CalcBase.Units
{
    /// <summary>
    /// SI base unit
    /// </summary>
    public record SIBaseUnit : ISIBaseUnit    
    {
        public string Name => Multiples.Single(m => m.Factor == 1).Name;
        public string[] Symbols => Multiples.Single(m => m.Factor == 1).Symbols;
        public IQuantity Quantity { get; init; }
        public UnitMultiple[] Multiples { get; init; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="quantity">Quantity</param>
        /// <param name="multiples">Unit multiples</param>
        public SIBaseUnit(IQuantity quantity, UnitMultiple[] multiples)
        {
            Quantity = quantity;
            Multiples = multiples.Select(m => m with { Parent = this }).ToArray();
        }
    }
}
