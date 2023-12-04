namespace CalcBase.Units
{
    /// <summary>
    /// Interface for SI units
    /// </summary>
    public interface ISIUnit : IUnit
    {
        UnitMultiple[] Multiples { get; }
    }
}
