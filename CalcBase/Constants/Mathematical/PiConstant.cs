using CalcBase.Numbers;

namespace CalcBase.Constants.Mathematical
{
    public record PiConstant : IConstant
    {
        public string Name => "Pi";
        public string Symbol => "π";
        public string SimpleSymbol => "pi";
        public Number Number => Number.Create(3.14159265358979323846m);
    }
}
