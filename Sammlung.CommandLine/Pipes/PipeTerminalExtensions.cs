using System;
using System.Linq.Expressions;
using Sammlung.CommandLine.Reflection;
using Sammlung.Pipes;

namespace Sammlung.CommandLine.Pipes
{
    [JetBrains.Annotations.PublicAPI]
    public static class PipeTerminalExtensions
    {
        public static IPipeTerminal<TData, T1> AsPipeTerminal<TData, T1, T2>
            (this IUnDiPipe<T1, T2> conversion, Expression<Func<TData, T2>> expression) =>
            conversion.AsPipeTerminal(default, expression);
        
        public static IPipeTerminal<TData, T1> AsPipeTerminal<TData, T1, T2>
            (this IUnDiPipe<T1, T2> conversion, IUnDiPipe<T2, T2> validation, Expression<Func<TData, T2>> expression)
        {
            var property = PropertyFactory.Property(expression);
            return conversion.AsPipeTerminal(validation, property);
        }

        public static IPipeTerminal<TData, T1> AsPipeTerminal<TData, T1, T2>
            (this IUnDiPipe<T1, T2> conversion, Func<TData, T2> getValue, Action<TData, T2> setValue) =>
            conversion.AsPipeTerminal(default, getValue, setValue);
        
        public static IPipeTerminal<TData, T1> AsPipeTerminal<TData, T1, T2>
            (this IUnDiPipe<T1, T2> conversion, IUnDiPipe<T2, T2> validation, Func<TData, T2> getValue, Action<TData, T2> setValue)
        {
            var property = new Property<TData, T2>(getValue, setValue);
            return new PipeTerminal<TData, T1, T2>(conversion, validation, property);
        }
        
        public static IPipeTerminal<TData, T1> AsPipeTerminal<TData, T1, T2>
            (this IUnDiPipe<T1, T2> conversion, IUnDiPipe<T2, T2> validation, Property<TData, T2> property) =>
            new PipeTerminal<TData, T1, T2>(conversion, validation, property);
    }
}