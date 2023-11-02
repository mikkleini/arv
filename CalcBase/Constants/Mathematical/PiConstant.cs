using CalcBase.Numbers;
using CalcBase.Units.Physics.Imperial;

namespace CalcBase.Constants.Mathematical
{
    /// <summary>
    /// Pi
    /// </summary>
    public record PiConstant : Singleton<PiConstant>, IConstant
    {
        public string Name => "Pi";
        public string Symbol => "π";
        public string SimpleSymbol => "pi";
        public Number Number => Number.Create(3.14159265358979323846m);
    }
}
