namespace Sammlung.Utilities.Math
{
    /// <summary>
    /// The <see cref="IntOperationProvider"/> class implements the <see cref="IOperationProvider{T}"/> for the int type.
    /// </summary>
    internal class IntOperationProvider : IOperationProvider<int>
    {
        /// <inheritdoc />
        public short NumBits => 31;

        /// <inheritdoc />
        public int Add(int lhs, long rhs) => (int) (lhs + rhs);

        /// <inheritdoc />
        public int Subtract(int lhs, long rhs) => (int) (lhs - rhs);

        /// <inheritdoc />
        public int ShiftRight(int lhs, long rhs) => lhs >> (int)rhs;

        /// <inheritdoc />
        public int Or(int lhs, int rhs) => lhs | rhs;
    }
}