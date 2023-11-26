using CalcBase.Quantities.Physical;
using CalcBase.Quantities;
using CalcBase.Units.Physics.SI;

namespace CalcBase.Units.Physics.Imperial
{
    /// <summary>
    /// Imperial mile per hour
    /// </summary>
    public record MilePerHourUnit : Singleton<InchUnit>, IConvertibleUnit
    {
        public string Name => "Mile per hour";
        public string Symbol => "mph";
        public string SimpleSymbol => "mph";
        public IQuantity Quantity => SpeedQuantity.Instance;
        public UnitConversionRule[] Conversions => new UnitConversionRule[]
        {
            UnitConversionRule.Create(MetrePerSecondUnit.Instance, new NumberType(0.0254M))
        };
    }
}
