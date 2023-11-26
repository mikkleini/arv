using CalcBase.Quantities;

namespace CalcBase.Units
{
    /// <summary>
    /// Interface for Imperial units
    /// </summary>
    public interface IImperialUnit : IUnit
    {
        NumberType EqualSIValue { get; }
        ISIUnit EqualSIUnit { get; }
    }
}
