namespace CalcBase.Quantities.Physical
{
    /// <summary>
    /// Length quantity
    /// </summary>
    public record LengthQuantity : Singleton<LengthQuantity>, IQuantity
    {
        public string Name => "Length";
        public string Symbol => "l";
        public string SimpleSymbol => "l";
    }
}
