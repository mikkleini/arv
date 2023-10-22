namespace CalcBase.Constants.Physical
{
    public class SpeedOfLightConstant : IPhysicsConstant
    {
        public string Name => "Speed of light in vacuum";
        public string Symbol => "c";
        public RealType Value => 299792458.0m;
        public string Unit => "m/s";
    }
}
