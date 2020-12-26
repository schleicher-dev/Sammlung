using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace _Fixtures.Sammlung.Extras
{
    [ExcludeFromCodeCoverage]
    public static class ExceptionTestHelper
    {
        public static IEnumerable<Type> CollectExceptionTypes(Assembly assembly) 
            => assembly.ExportedTypes.Where(t => typeof(Exception).IsAssignableFrom(t));
    }
}