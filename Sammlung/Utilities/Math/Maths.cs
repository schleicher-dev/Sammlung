namespace Sammlung.Utilities.Math
{
    internal static class Maths
    {
        private static T GenericNextPowerOf2<T>(T value, IOperationProvider<T> provider) where T : struct
        {
            value = provider.Sub(value, 1);
            for (var i = 1; i <= provider.NumBits; i <<= 1) 
                value = provider.Or(value, provider.ShiftRight(value, i));
            return provider.Add(value, 1);
        }

        public static int NextPowerOf2(int v) => GenericNextPowerOf2(v, IntOperationProvider.Instance);
    }
}