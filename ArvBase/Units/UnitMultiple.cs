using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArvBase.Units
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
        public bool UseForDisplay { get; init; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="symbols">Symbols</param>
        /// <param name="factor">Factor</param>
        /// <param name="useForDisplay">Use for display</param>
        /// <param name="context">Context</param>
        public UnitMultiple(string name, string[] symbols, NumberType factor, UnitContext context = UnitContext.All, bool useForDisplay = true)
        {
            Parent = Factory.DummyUnit;
            Name = name;
            Symbols = symbols;
            Factor = factor;
            Context = context;
            UseForDisplay = useForDisplay;
        }

        /// <summary>
        /// Custom print members function to avoid recurssive parent unit multiples printing
        /// </summary>
        /// <param name="stringBuilder">Stringbuilder</param>
        /// <returns>true if printed</returns>
        protected virtual bool PrintMembers(StringBuilder stringBuilder)
        {
            stringBuilder.Append($"Parent = \"{Parent.Name}\", ");
            stringBuilder.Append($"Name = \"{Name}\", ");
            stringBuilder.Append($"Symbols = [{string.Join(',', Symbols.Select(s => $"\"{s}\""))}], ");
            stringBuilder.Append($"Factor = {Factor}, ");
            stringBuilder.Append($"Context = {Context}, ");
            stringBuilder.Append($"UseForDisplay = {UseForDisplay}");
            return true;
        }
    }
}
