using System;
using Sammlung.Resources;

namespace Sammlung.Utilities
{
    internal static class ArgCheckExtensions
    {
        public static T RequireNotNull<T>(this T input, string argName) where T : class
            => input ?? throw new ArgumentNullException(argName);

        public static T RequireStrictlyPositive<T>(this T input, string argName) where T : IComparable<T> =>
            0 < input.CompareTo(default)
                ? input
                : throw new ArgumentOutOfRangeException(argName, input, ErrorMessages.ValueMustBeStrictlyPositive);
    }
}