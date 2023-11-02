namespace CalcBase.Units
{
    /// <summary>
    /// Interface for convertible units
    /// </summary>
    public interface IConvertibleUnit : IUnit
    {
        UnitConversionRule[] Conversions { get; }
    }
}
