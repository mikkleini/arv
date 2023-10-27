using CalcBase.Numbers;

namespace CalcBase.Constants.Physical
{
    public record SpeedOfLightConstant : IPhysicsConstant
    {
        public string Name => "Speed of light in vacuum";
        public string Symbol => "c";
        public string SimpleSymbol => "c";
        public Number Number => Number.Create(299792458.0m);
        public string Unit => "m/s";
    }
}
