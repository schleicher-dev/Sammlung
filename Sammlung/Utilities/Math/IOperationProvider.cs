namespace Sammlung.Utilities.Math
{
    internal interface IOperationProvider<T> where T : struct
    {
        public short NumBits { get; }
        public T Add(T lhs, long rhs);
        public T Sub(T lhs, long rhs);
        public T ShiftRight(T lhs, long rhs);
        public T Or(T lhs, T rhs);
    }
}