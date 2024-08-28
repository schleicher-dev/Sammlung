using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;
using Sammlung.Numerics;
using Sammlung.Pipes.Werkzeug.Exceptions;
using Sammlung.Pipes.Werkzeug.Validators;

namespace Fixtures.Sammlung.Pipes.Werkzeug
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [ExcludeFromCodeCoverage]
    [SetCulture("")]
    public class WerkzeugValidationPipeTests
    {
        private static IEnumerable<Interval<double>> CreateIntervals(IReadOnlyList<double> ranges)
        {
            Assert.That(ranges.Count % 2, Is.EqualTo(0), "Ranges should come in pairs. In this case they are not.");
            for (var i = 0; i < ranges.Count; i += 2)
            {
                yield return Interval.CreateWithInclusiveBounds(ranges[i], ranges[i + 1]);
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

        [TestCase(5.1d, -5d, 5d)]
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

        [TestCase(@"^TwoNumbers\d{2}$", "TwoNumbers11", "TwoNumbers92")]
        [TestCase(@"^TwoNumbers\d{2}", "TwoNumbers113", "TwoNumbers9999")]
        public void Matching_RegexValidationTests(string regularExpr, params string[] matchCandidates)
        {
            var regex = new Regex(regularExpr);
            var pipe = new RegexValidatorPipe(regex);

            Assert.That(matchCandidates, Is.Not.Empty);
            foreach (var matchCandidate in matchCandidates)
                Assert.DoesNotThrow(() => pipe.ProcessForward(matchCandidate));
        }
        
        [TestCase(@"^TwoNumbers\d{2}$", "TwoNumbers113", "TwoNumbers9")]
        public void NonMatching_RegexValidationTests(string regularExpr, params string[] matchCandidates)
        {
            var regex = new Regex(regularExpr);
            var pipe = new RegexValidatorPipe(regex);

            Assert.That(matchCandidates, Is.Not.Empty);
            foreach (var matchCandidate in matchCandidates)
                Assert.Throws<PipelineValidationException>(() => pipe.ProcessForward(matchCandidate));
        }
    }
}