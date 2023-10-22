namespace CalcBase.Constants.Mathematical
{
    public record PiConstant : IRealConstant
    {
        public string Name => "Pi";
        public string Symbol => "π";
        public RealType Value => 3.14159265358979323846m;
    }
}
