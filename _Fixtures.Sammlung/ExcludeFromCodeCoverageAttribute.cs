using System;

namespace _Fixtures.Sammlung
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
    internal sealed class ExcludeFromCodeCoverageAttribute : Attribute
    {
    }
}