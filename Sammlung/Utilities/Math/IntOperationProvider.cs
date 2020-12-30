using System;
using System.Threading;

namespace Sammlung.Utilities.Math
{
    internal class IntOperationProvider : IOperationProvider<int>
    {
        private static readonly Lazy<IOperationProvider<int>> Loader =
            new Lazy<IOperationProvider<int>>(() => new IntOperationProvider(),
                LazyThreadSafetyMode.PublicationOnly);

        public static IOperationProvider<int> Instance => Loader.Value;
        
        private IntOperationProvider() {}
        
        public short NumBits => 31;

        public int Add(int lhs, long rhs) => (int) (lhs + rhs);

        public int Sub(int lhs, long rhs) => (int) (lhs - rhs);

        public int ShiftRight(int lhs, long rhs) => lhs >> (int)rhs;

        public int Or(int lhs, int rhs) => lhs | rhs;
    }
}