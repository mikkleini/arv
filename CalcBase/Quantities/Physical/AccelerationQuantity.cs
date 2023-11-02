namespace CalcBase.Quantities.Physical
{
    /// <summary>
    /// Acceleration quantity
    /// </summary>
    public record AccelerationQuantity : Singleton<VelocityQuantity>, IQuantity
    {
        public string Name => "Acceleration";
        public string Symbol => "a";
        public string SimpleSymbol => "a";
    }
}
