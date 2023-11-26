using CalcBase.Quantities.Physical;
using CalcBase.Quantities;
using CalcBase.Quantities.Mathematical;

namespace CalcBase.Units.Physics.SI
{
    /// <summary>
    /// SI square meter
    /// </summary>
    public record SquareMeterUnit : Singleton<SquareMeterUnit>, ISIDerivedUnit
    {
        public string Name => "Square meter";
        public string Symbol => "m^2";
        public string SimpleSymbol => "m^2";
        public IQuantity Quantity => AreaQuantity.Instance;
    }
}
