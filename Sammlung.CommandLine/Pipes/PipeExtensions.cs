using System;
using System.Text.RegularExpressions;
using Sammlung.Numerics;
using Sammlung.Pipes;
using Sammlung.Pipes.Werkzeug.Validators;

namespace Sammlung.CommandLine.Pipes
{
    /// <summary>
    /// The <see cref="PipeExtensions"/> type exposes extension methods for manipulating and composing
    /// <see cref="IUnDiPipe{TSource,TTarget}"/> instances.
    /// </summary>
    [JetBrains.Annotations.PublicAPI]
    public static class PipeExtensions
    {
        /// <summary>
        /// Appends a <see cref="IUnDiPipe{TSource,TTarget}"/> which removes surrounding double quotes from a string.
        /// </summary>
        /// <param name="pipe">the input pipe</param>
        /// <returns>the new pipe</returns>
        public static IUnDiPipe<T, string> WithDoubleQuotesRemoval<T>(this IUnDiPipe<T, string> pipe) =>
            pipe.Append(new RemoveDoubleQuotesPipe());

        /// <summary>
        /// Appends a <see cref="IUnDiPipe{TSource,TTarget}"/> which validates that the pipe values
        /// are in the configured intervals passed into this method.
        /// </summary>
        /// <param name="pipe">the source pipe</param>
        /// <param name="intervals">the intervals to check</param>
        /// <typeparam name="T1">the pipe input type</typeparam>
        /// <typeparam name="T2">the pipe output type</typeparam>
        /// <returns>the new pipe</returns>
        public static IUnDiPipe<T1, T2> WithIntervalValidator<T1, T2>(this IUnDiPipe<T1, T2> pipe, params Interval<T2>[] intervals)
            where T2 : IComparable<T2>
        {
            var validator = new IntervalValidatorPipe<T2>(intervals);
            return pipe.Append(validator.ForwardPipe());
        }

        /// <summary>
        /// Appends a <see cref="IUnDiPipe{TSource,TTarget}"/> which validates that the pipe values
        /// adhere to a given regular expression.
        /// </summary>
        /// <param name="pipe">the pipe</param>
        /// <param name="regex">the regular expression</param>
        /// <typeparam name="T1">the pipe input type</typeparam>
        /// <returns>the new pipe</returns>
        public static IUnDiPipe<T1, string> WithRegexValidator<T1>(this IUnDiPipe<T1, string> pipe, string regex) => 
            pipe.WithRegexValidator(new Regex(regex, RegexOptions.Compiled));

        /// <summary>
        /// Appends a <see cref="IUnDiPipe{TSource,TTarget}"/> which validates that the pipe values
        /// adhere to a given regular expression.
        /// </summary>
        /// <param name="pipe">the pipe</param>
        /// <param name="regex">the regular expression</param>
        /// <typeparam name="T1">the pipe input type</typeparam>
        /// <returns>the new pipe</returns>
        public static IUnDiPipe<T1, string> WithRegexValidator<T1>(this IUnDiPipe<T1, string> pipe, Regex regex)
        {
            var validator = new RegexValidatorPipe(regex);
            return pipe.Append(validator.ForwardPipe());
        }
    }
}