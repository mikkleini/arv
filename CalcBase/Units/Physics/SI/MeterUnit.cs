using CalcBase.Quantities;
using CalcBase.Quantities.Physical;

namespace CalcBase.Units.Physics.SI
{
    /// <summary>
    /// SI meter
    /// </summary>
    public record MeterUnit : Singleton<MeterUnit>, ISIBaseUnit
    {
        public string Name => "Meter";
        public string Symbol => "m";
        public string SimpleSymbol => "m";
        public IQuantity Quantity => LengthQuantity.Instance;
    }
}
