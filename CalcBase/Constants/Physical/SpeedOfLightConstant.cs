using CalcBase.Numbers;
using CalcBase.Units;
using CalcBase.Units.Physics.SI;

namespace CalcBase.Constants.Physical
{
    /// <summary>
    /// Speed of light in vacuum
    /// </summary>
    public record SpeedOfLightConstant : Singleton<SpeedOfLightConstant>, IPhysicsConstant
    {
        public string Name => "Speed of light in vacuum";
        public string Symbol => "c";
        public string SimpleSymbol => "c";
        public Number Number => Number.Create(299792458.0m);
        public IUnit Unit => MetrePerSecondUnit.Instance;
    }
}
