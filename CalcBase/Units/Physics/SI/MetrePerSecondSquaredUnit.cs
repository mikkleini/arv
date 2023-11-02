using CalcBase.Quantities.Physical;
using CalcBase.Quantities;

namespace CalcBase.Units.Physics.SI
{
    /// <summary>
    /// SI metre per second squared
    /// </summary>
    public record MetrePerSecondSquaredUnit : Singleton<MetrePerSecondUnit>, ISIDerivedUnit
    {
        public string Name => "Metre per second squared";
        public string Symbol => "m/s^2";
        public string SimpleSymbol => "m/s^2";
        public IQuantity Quantity => AccelerationQuantity.Instance;
    }
}
