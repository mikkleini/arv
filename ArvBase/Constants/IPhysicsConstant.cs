using ArvBase.Quantities;
using ArvBase.Units;

namespace ArvBase.Constants
{
    /// <summary>
    /// Physics constant
    /// </summary>
    interface IPhysicsConstant : IConstant
    {
        IUnit Unit { get; }
    }
}
