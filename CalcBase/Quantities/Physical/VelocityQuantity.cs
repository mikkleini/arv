namespace CalcBase.Quantities.Physical
{
    /// <summary>
    /// Velocity quantity
    /// </summary>
    public record VelocityQuantity : Singleton<VelocityQuantity>, IQuantity
    {
        public string Name => "Velocity";
        public string Symbol => "v";
        public string SimpleSymbol => "v";
    }
}
