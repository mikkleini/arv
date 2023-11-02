using CalcBase.Quantities.Physical;
using CalcBase.Quantities;
using CalcBase.Units.Physics.SI;

namespace CalcBase.Units.Physics.Imperial
{
    /// <summary>
    /// Imperial foot
    /// </summary>
    public record FootUnit : Singleton<FootUnit>, IConvertibleUnit
    {
        public string Name => "Foot";
        public string Symbol => "'";
        public string SimpleSymbol => "ft";
        public IQuantity Quantity => LengthQuantity.Instance;
        public UnitConversionRule[] Conversions => new UnitConversionRule[]
        {
            UnitConversionRule.Create(MeterUnit.Instance, new NumberType(0.3048M)),
            UnitConversionRule.Create(InchUnit.Instance, new NumberType(12)),
        };
    }
}
