using CalcBase.Numbers;

namespace CalcBase.Constants
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
