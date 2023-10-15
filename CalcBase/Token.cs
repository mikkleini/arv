using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcBase
{
    

    public record Token
    {
        public required TokenType Type { get; init; }
        public required int Length { get; init; }
    }

    public record BinaryNumberToken : Token
    {
        public required IntType Value { get; init; }
    }

    public record IntegerNumberToken : Token
    {
        public required IntType Value { get; init; }
    }

    public record HexadecimalNumberToken : Token
    {
        public required IntType Value { get; init; }
        public required DominantCase DominantCase { get; init; }
    }

    public record RealNumberToken : Token
    {
        public required RealType Value { get; init; }
    }
}
