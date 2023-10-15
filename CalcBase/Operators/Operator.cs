using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcBase.Operators
{
    public record Operator
    {
        /// <summary>
        /// Name
        /// </summary>
        public required string Name { get; init; }

        /// <summary>
        /// Precedence
        /// </summary>
        public required int Precedence { get; init; }

        /// <summary>
        /// TBD
        /// </summary>
        public bool RightAssociative { get; init; } = false;
    }
}
