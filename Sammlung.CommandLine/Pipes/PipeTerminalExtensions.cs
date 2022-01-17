using System;
using Sammlung.CommandLine.Utilities;
using Sammlung.Pipes;

namespace Sammlung.CommandLine.Pipes
{
    [JetBrains.Annotations.PublicAPI]
    public static class PipeTerminalExtensions
    {
        public static IPipeTerminal<TData, T1> AsPipeTerminal<TData, T1, T2>
            (this IUnDiPipe<T1, T2> pipe, Action<TData, T2> setValue) =>
            pipe.AsPipeTerminal(new BindableSetter<TData, T2>(setValue));
        
        public static IPipeTerminal<TData, T1> AsPipeTerminal<TData, T1, T2>
            (this IUnDiPipe<T1, T2> pipe, BindableSetter<TData, T2> setValue) =>
            new PipeTerminal<TData,T1,T2>(pipe, setValue);
    }
}