using System;
using System.Linq;
using Sammlung.Numerics;
using Sammlung.Pipes.Werkzeug.Exceptions;
using Sammlung.Pipes.Werkzeug.Resources;
using Sammlung.Werkzeug;

namespace Sammlung.Pipes.Werkzeug.Validators
{
    /// <summary>
    /// The <see cref="IntervalValidatorPipe"/> is a static class containing a factory method for
    /// <see cref="IntervalValidatorPipe{T}"/>.
    /// </summary>
    [JetBrains.Annotations.PublicAPI]
    public static class IntervalValidatorPipe
    {
        /// <summary>
        /// Creates a new <see cref="IntervalValidatorPipe{T}"/> using at least one interval.
        /// </summary>
        /// <param name="intervals">the intervals</param>
        /// <typeparam name="T">the type to check</typeparam>
        /// <returns>the pipe</returns>
        public static IBiDiPipe<T, T> Create<T>(params Interval<T>[] intervals) 
            where T : IComparable<T> => new IntervalValidatorPipe<T>(intervals);
    }
    
    /// <summary>
    /// The <see cref="IntervalValidatorPipe{T}"/> validates if a particular value is in a set of intervals.
    /// </summary>
    /// <typeparam name="T">the value type</typeparam>
    [JetBrains.Annotations.PublicAPI]
    public class IntervalValidatorPipe<T> : ValidatorPipeBase<T> where T : IComparable<T>
    {
        private readonly Interval<T>[] _intervals;

        /// <summary>
        /// Creates a new <see cref="IntervalValidatorPipe{T}"/> using a number of intervals.
        /// </summary>
        /// <param name="intervals">the intervals</param>
        public IntervalValidatorPipe(params Interval<T>[] intervals)
        {
            _intervals = intervals.RequireNotNull(nameof(intervals)).RequireAtLeastNumElements(1, nameof(intervals));
        }

        private string GetIntervalErrorMessage()
        {
            if (_intervals.Length == 1)
                return string.Format(Lang.Validation_Interval_Exc_SingleInterval, _intervals.First());
            var intervalList = string.Join(", ", _intervals.Select(i => i.ToString()).ToArray());
            return string.Format(Lang.Validation_Interval_Exc_MultiInterval, intervalList);
        }
        
        /// <inheritdoc />
        protected override PipelineValidationException GetException(T value)
        {
            var message = string.Format(Lang.Validation_Interval_Exc_Reason, value, GetIntervalErrorMessage());
            return new PipelineValidationException(message);
        }

        /// <inheritdoc />
        protected override bool TryValidate(T value) =>_intervals.Any(i => i.Contains(value));

    }
}