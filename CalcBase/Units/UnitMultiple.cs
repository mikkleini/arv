using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcBase.Units
{
    /// <summary>
    /// Unit multiple
    /// </summary>
    public record UnitMultiple : IElement
    {
        public IUnit Parent { get; init; }
        public string Name { get; init; }
        public string[] Symbols { get; init; }
        public NumberType Factor { get; init; }
        public UnitContext Context { get; init; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="symbols">Symbols</param>
        /// <param name="factor">Factor</param>
        /// <param name="context">Context</param>
        public UnitMultiple(string name, string[] symbols, NumberType factor, UnitContext context = UnitContext.All)
        {
            Parent = Factory.DummyUnit;
            Name = name;
            Symbols = symbols;
            Factor = factor;
            Context = context;
        }

        /// <summary>
        /// Custom ToString method because "Parent" property makes it recurssive.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
            (this with { Parent = Factory.DummyUnit }).PrintMembers(builder);
            return builder.ToString();
        }
    }
}
