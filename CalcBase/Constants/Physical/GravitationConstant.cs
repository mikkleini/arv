using CalcBase.Numbers;

namespace CalcBase.Constants.Physical
{
    public record GravitationConstant : IPhysicsConstant
    {
        public string Name => "Newtonian constant of gravitation";
        public string Symbol => "G";
        public string SimpleSymbol => "G";
        public Number Number => Number.Create(6.6743e-11m, isScientificNotation: true);
        public string Unit => "m^3*kg^−1*s^−2";
    }
}
