using System;
using System.Linq.Expressions;
using Sammlung.Pipes;

namespace Sammlung.CommandLine.Pipes
{
    [JetBrains.Annotations.PublicAPI]
    public static class PipeTerminalExtensions
    {
        private static Action<T1, T2> CreateSetValue<T1, T2>(Expression<Func<T1, T2>> getValue)
        {
            var memberExpr = (MemberExpression)getValue.Body;
            var @this = Expression.Parameter(typeof(T1), "$this");
            var value = Expression.Parameter(typeof(T2), "value");

            var assignExpression = Expression.Assign(Expression.MakeMemberAccess(@this, memberExpr.Member), value);
            var lambdaExpression = Expression.Lambda<Action<T1, T2>>(assignExpression, @this, value);
            return lambdaExpression.Compile();
        }

        public static IPipeTerminal<TData, T1> AsPipeTerminal<TData, T1, T2>
            (this IUnDiPipe<T1, T2> conversion, Expression<Func<TData, T2>> expression) =>
            conversion.AsPipeTerminal(default, expression);
        
        public static IPipeTerminal<TData, T1> AsPipeTerminal<TData, T1, T2>
            (this IUnDiPipe<T1, T2> conversion, IUnDiPipe<T2, T2> validation, Expression<Func<TData, T2>> expression)
        {
            var getValue = expression.Compile();
            var setValue = CreateSetValue(expression);
            return conversion.AsPipeTerminal(validation, getValue, setValue);
        }

        public static IPipeTerminal<TData, T1> AsPipeTerminal<TData, T1, T2>
            (this IUnDiPipe<T1, T2> conversion, Func<TData, T2> getValue, Action<TData, T2> setValue) =>
            conversion.AsPipeTerminal(default, getValue, setValue);
        
        public static IPipeTerminal<TData, T1> AsPipeTerminal<TData, T1, T2>
            (this IUnDiPipe<T1, T2> conversion, IUnDiPipe<T2, T2> validation, Func<TData, T2> getValue, Action<TData, T2> setValue) =>
            new PipeTerminal<TData, T1, T2>(conversion, validation, getValue, setValue);
    }
}