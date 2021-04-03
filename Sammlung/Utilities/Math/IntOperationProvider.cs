using JetBrains.Annotations;

namespace Sammlung.Utilities.Math
{
    [PublicAPI]
    internal class IntOperationProvider : IOperationProvider<int>
    {
        public short NumBits => 31;

        public int Add(int lhs, long rhs) => (int) (lhs + rhs);

        public int Sub(int lhs, long rhs) => (int) (lhs - rhs);

        public int ShiftRight(int lhs, long rhs) => lhs >> (int)rhs;

        public int Or(int lhs, int rhs) => lhs | rhs;
    }
}