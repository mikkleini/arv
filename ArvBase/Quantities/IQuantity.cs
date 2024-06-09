namespace ArvBase.Quantities
{
    /// <summary>
    /// Quantity
    /// </summary>
    public interface IQuantity : IElement
    {
        string Name { get; }
        string[] Symbols { get; }
    }
}
