using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcBase
{
    public record Result<T, E>        
    {
        /// <summary>
        /// Value
        /// </summary>
        public T? Value { get; init; }

        /// <summary>
        /// Error
        /// </summary>
        public E? Error { get; init; }
    }
}
