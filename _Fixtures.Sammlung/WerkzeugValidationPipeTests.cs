using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Sammlung.Numerics;
using Sammlung.Pipes.Werkzeug.Exceptions;
using Sammlung.Pipes.Werkzeug.Validators;

namespace _Fixtures.Sammlung
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    [SetCulture("")]
    public class WerkzeugValidationPipeTests
    {
        private static IEnumerable<Interval<double>> CreateIntervals(IReadOnlyList<double> ranges)
        {
            Assert.AreEqual(0, ranges.Count % 2, "Ranges should come in pairs. In this case they are not.");
            for (var i = 0; i < ranges.Count; i+=2)
            {
                var lb = Bound.Inclusive(ranges[i]);
                var ub = Bound.Inclusive(ranges[i + 1]);
                yield return Interval.Create(lb, ub);
            }
        }

        [TestCase(2d, -5d, 5d)]
        [TestCase(2d, -5d, 5d, 15d, 25d)]
        [TestCase(20d, -5d, 5d, 15d, 25d)]
        public void ValueInMultiRange_DoesNotThrow(double value, params double[] ranges)
        {
            var intervals = CreateIntervals(ranges);
            var validatorPipe = IntervalValidatorPipe.Create(intervals.ToArray());
            
            Assert.DoesNotThrow(() => validatorPipe.ProcessForward(value));
            Assert.DoesNotThrow(() => validatorPipe.ProcessReverse(value));
        }
        
        [TestCase( 5.1d, -5d, 5d)]
        [TestCase(-7d, -5d, 5d, 15d, 25d)]
        [TestCase(6d, -5d, 5d, 15d, 25d)]
        [TestCase(25.0001d, -5d, 5d, 0d, 25d)]
        public void ValueOutOfMultiRange_ThrowsPipelineValidationException(double value, params double[] ranges)
        {
            var intervals = CreateIntervals(ranges);
            var validatorPipe = IntervalValidatorPipe.Create(intervals.ToArray());
            
            Assert.Throws<PipelineValidationException>(() => validatorPipe.ProcessForward(value));
            Assert.Throws<PipelineValidationException>(() => validatorPipe.ProcessReverse(value));
        }
    }
}