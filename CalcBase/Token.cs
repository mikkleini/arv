using CalcBase.Operators;

namespace CalcBase
{
    /// <summary>
    /// Token
    /// </summary>
    public abstract record Token
    {
        public virtual TokenType Type { get; init; }
        public int Position { get; init; }
        public int Length { get; init; }
    }

    /// <summary>
    /// Binary number token
    /// </summary>
    public record BinaryNumberToken : Token
    {
        public override TokenType Type => TokenType.BinaryNumber;
        public required IntType Value { get; init; }
    }

    /// <summary>
    /// Hexadecimal number token
    /// </summary>
    public record HexadecimalNumberToken : Token
    {
        public override TokenType Type => TokenType.HexadecimalNumber;
        public required IntType Value { get; init; }
        public required DominantCase DominantCase { get; init; }
    }

    /// <summary>
    /// Integer number token
    /// </summary>
    public record IntegerNumberToken : Token
    {
        public override TokenType Type => TokenType.IntegerNumber;
        public required IntType Value { get; init; }
        public IntType Exponent { get; init; } = 1;
        public bool IsScientificNotation { get; init; } = false;
    }

    /// <summary>
    /// Real number token
    /// </summary>
    public record RealNumberToken : Token
    {
        public override TokenType Type => TokenType.RealNumber;
        public required RealType Value { get; init; }
        public IntType Exponent { get; init; } = 1;
        public bool IsScientificNotation { get; init; } = false;
    }

    /// <summary>
    /// Text token
    /// </summary>
    public record TextToken : Token
    {
        public override TokenType Type => TokenType.Text;
        public required string Text { get; init; }
    }

    /// <summary>
    /// Parenthesis token
    /// </summary>
    public record ParenthesisToken : Token
    {
        public override TokenType Type => TokenType.Parenthesis;
        public required Side Side { get; init; }
    }

    /// <summary>
    /// Operator token
    /// </summary>
    public record OperatorToken : Token
    {
        public override TokenType Type => TokenType.Operator;
        public required IOperator Operator { get; init; }
    }
}
