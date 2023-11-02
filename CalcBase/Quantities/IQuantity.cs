namespace CalcBase.Quantities
{
    /// <summary>
    /// Quantity
    /// </summary>
    public interface IQuantity
    {
        string Name { get; }
        string Symbol { get; }
        string SimpleSymbol { get; }
    }
}
