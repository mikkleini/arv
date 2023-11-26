using CalcBase.Quantities;
using CalcBase.Units;

namespace CalcBase.Constants
{
    /// <summary>
    /// Physics constant
    /// </summary>
    interface IPhysicsConstant : IConstant
    {
        IUnit Unit { get; }
    }
}
