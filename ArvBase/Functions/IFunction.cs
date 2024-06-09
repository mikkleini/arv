namespace ArvBase.Functions
{
    /// <summary>
    /// Function
    /// </summary>
    public interface IFunction : IElement
    {
        string Name { get; }
        string Symbol { get; }
    }
}
