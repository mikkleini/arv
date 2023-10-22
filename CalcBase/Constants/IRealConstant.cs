namespace CalcBase.Constants
{
    /// <summary>
    /// Interface for real number constant
    /// </summary>
    interface IRealConstant : IConstant
    {
        RealType Value { get; }
    }
}
