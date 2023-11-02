using CalcBase.Quantities.Physical;
using CalcBase.Quantities;
using CalcBase.Units.Physics.SI;

namespace CalcBase.Units.Physics.Imperial
{
    /// <summary>
    /// Imperial inch
    /// </summary>
    public record InchUnit : Singleton<InchUnit>, IConvertibleUnit
    {
        public string Name => "Inch";
        public string Symbol => "\"";
        public string SimpleSymbol => "in";
        public IQuantity Quantity => LengthQuantity.Instance;
        public UnitConversionRule[] Conversions => new UnitConversionRule[]
        {
            UnitConversionRule.Create(MeterUnit.Instance, new NumberType(0.0254M)),
            UnitConversionRule.Create(FootUnit.Instance, new NumberType(1M / 12M)),
        };
    }
}
