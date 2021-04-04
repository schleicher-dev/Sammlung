namespace Sammlung.Utilities.Math
{
    /// <summary>
    /// The <see cref="Maths"/> class contains some very useful mathematical operations.
    /// </summary>
    internal static class Maths
    {
        private static T GenericNextPowerOf2<T>(T value, IOperationProvider<T> provider) where T : struct
        {
            value = provider.Subtract(value, 1);
            for (var i = 1; i <= provider.NumBits; i <<= 1) 
                value = provider.Or(value, provider.ShiftRight(value, i));
            return provider.Add(value, 1);
        }

        /// <summary>
        /// Calculates the next power of two which is bigger than the passed value.
        /// </summary>
        /// <param name="v">the value</param>
        /// <returns>the next power of two</returns>
        public static int NextPowerOf2(int v) => GenericNextPowerOf2(v, new IntOperationProvider());
    }
}