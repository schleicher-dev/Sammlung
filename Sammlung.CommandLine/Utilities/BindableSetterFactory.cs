using System;

namespace Sammlung.CommandLine.Utilities
{
    public static class BindableSetterFactory
    {
        public static BindableSetter<T1, T2> Create<T1, T2>(Action<T1, T2> setterAction) => 
            new BindableSetter<T1, T2>(setterAction);
    }
}