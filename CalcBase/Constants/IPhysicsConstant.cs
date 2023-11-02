using CalcBase.Quantities;
using CalcBase.Units;

namespace CalcBase.Constants
{
    /// <summary>
    /// Interface for physics constant
    /// </summary>
    interface IPhysicsConstant : IConstant
    {
        IUnit Unit { get; }
    }
}
