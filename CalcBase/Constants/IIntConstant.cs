namespace CalcBase.Constants
{
    /// <summary>
    /// Interface for integer number constant
    /// </summary>
    interface IIntConstant : IConstant
    {
        IntType Value { get; }
    }
}
