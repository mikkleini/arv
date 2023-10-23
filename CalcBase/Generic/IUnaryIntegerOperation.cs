namespace CalcBase.Generic
{
    public interface IUnaryIntegerOperation
    {
        IntType Calculate(IntType a, out bool requireRealOp);
    }
}
