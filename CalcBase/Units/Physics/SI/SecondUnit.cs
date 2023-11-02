using CalcBase.Quantities.Physical;
using CalcBase.Quantities;

namespace CalcBase.Units.Physics.SI
{
    /// <summary>
    /// SI second
    /// </summary>
    public record SecondUnit : Singleton<SecondUnit>, ISIBaseUnit
    {
        public string Name => "Second";
        public string Symbol => "s";
        public string SimpleSymbol => "s";
        public IQuantity Quantity => TimeQuantity.Instance;
    }
}
