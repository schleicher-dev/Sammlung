using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Sammlung.Numerics;
using Sammlung.Pipes;
using Sammlung.Pipes.Werkzeug.Validators;

namespace Sammlung.CommandLine.Pipes
{
    public static class PipeExtensions
    {
        public static IUnDiPipe<IEnumerable<T1>, IEnumerable<T2>> WithMultiInput<T1, T2>(this IUnDiPipe<T1, T2> pipe)
            => new EnumerablePipe<T1, T2>(pipe);

        public static IUnDiPipe<IEnumerable<T1>, T2> WithUniqueOutput<T1, T2>
            (this IUnDiPipe<IEnumerable<T1>, IEnumerable<T2>> pipe, T2 defaultValue = default)
        {
            var projection = new ProjectionPipe<T2>(defaultValue);
            return pipe.Concat(projection);
        }

        public static IUnDiPipe<string, string> WithDoubleQuotesRemoval(this IUnDiPipe<string, string> pipe) =>
            pipe.Concat(new RemoveDoubleQuotesPipe());

        public static IUnDiPipe<T1, T2> WithIntervalValidator<T1, T2>(this IUnDiPipe<T1, T2> pipe, params Interval<T2>[] intervals)
            where T2 : IComparable<T2>
        {
            var validator = new IntervalValidatorPipe<T2>(intervals);
            return pipe.Concat(validator.ForwardPipe());
        }

        public static IUnDiPipe<T1, string> WithRegexValidator<T1>(this IUnDiPipe<T1, string> pipe, string regex) => 
            pipe.WithRegexValidator(new Regex(regex, RegexOptions.Compiled));

        public static IUnDiPipe<T1, string> WithRegexValidator<T1>(this IUnDiPipe<T1, string> pipe, Regex regex)
        {
            var validator = new RegexValidatorPipe(regex);
            return pipe.Concat(validator.ForwardPipe());
        }
        
        public static IPipeEndpoint<TData, T1> AsPipeEndpoint<TData, T1, T2>(this IUnDiPipe<T1, T2> pipe, PushValueDelegate<TData, T2> pushValue) => 
            new PipeEndpoint<TData, T1, T2>(pipe, pushValue);


    }
}