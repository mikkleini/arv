using CalcBase.Constants.Mathematical;
using CalcBase.Numbers;
using CalcBase.Units;
using CalcBase.Units.Physics.SI;

namespace CalcBase.Constants.Physical
{
    public record GravitationConstant : Singleton<GravitationConstant>, IPhysicsConstant
    {
        public string Name => "Newtonian constant of gravitation";
        public string Symbol => "G";
        public string SimpleSymbol => "G";
        public Number Number => Number.Create(6.6743e-11m, isScientificNotation: true);
        public IUnit Unit => MeterUnit.Instance; // TODO "m^3*kg^−1*s^−2";
    }
}
