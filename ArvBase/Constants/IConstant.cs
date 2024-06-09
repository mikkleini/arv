using ArvBase.Numbers;

namespace ArvBase.Constants
{
    /// <summary>
    /// Constant
    /// </summary>
    public interface IConstant : IElement
    {
        string Name { get; }
        string[] Symbols { get; }
        Number Number { get; }
    }
}
