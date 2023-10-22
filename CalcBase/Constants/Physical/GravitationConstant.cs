namespace CalcBase.Constants.Physical
{
    public record GravitationConstant
    {
        public static string Name => "Newtonian constant of gravitation";
        public static string Symbol => "G";
        public static RealType Value => 6.6743e-11m;
        public static string Unit => "m^3*kg^−1*s^−2";
    }
}
