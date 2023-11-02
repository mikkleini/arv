using CalcBase.Quantities.Physical;
using CalcBase.Quantities;

namespace CalcBase.Units.Physics.SI
{
    /// <summary>
    /// SI metre per second
    /// </summary>
    public record MetrePerSecondUnit : Singleton<MetrePerSecondUnit>, ISIDerivedUnit
    {
        public string Name => "Metre per second";
        public string Symbol => "m/s";
        public string SimpleSymbol => "m/s";
        public IQuantity Quantity => VelocityQuantity.Instance;
    }
}
