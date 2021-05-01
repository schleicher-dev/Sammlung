using System;
using System.Runtime.CompilerServices;

namespace Sammlung.Utilities
{
    internal static class ArgCheckExtensions
    {
        public static T RequireNotNull<T>(this T input, string argName) where T : class
            => input ?? throw new ArgumentNullException(argName);
    }
}