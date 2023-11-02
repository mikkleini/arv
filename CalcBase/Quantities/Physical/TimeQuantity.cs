namespace CalcBase.Quantities.Physical
{
    /// <summary>
    /// Time quantity
    /// </summary>
    public record TimeQuantity : Singleton<TimeQuantity>, IQuantity
    {
        public string Name => "Time";
        public string Symbol => "t";
        public string SimpleSymbol => "t";
    }
}
