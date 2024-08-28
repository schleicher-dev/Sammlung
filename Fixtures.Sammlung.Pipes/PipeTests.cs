using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using NUnit.Framework;
using Sammlung.Pipes;
using Sammlung.Pipes.SpecialPipes;
using Sammlung.Pipes.Werkzeug.Converters;

namespace Fixtures.Sammlung.Pipes
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [ExcludeFromCodeCoverage]
    public class PipeTests
    {
        private const string Format = "N16";
        private const double Tolerance = 1e-12;
        
        [Test]
        public void BiDi_BiDi_Concatenation([Random(-100d, 100d, 20)] double value)
        {
            var pipe = new StringToDoubleConverterPipe(Format, CultureInfo.InvariantCulture);
            var identity = pipe.Invert().Append(pipe.Invert().Invert());

            Assert.That(identity.ProcessForward(value), Is.EqualTo(value).Within(Tolerance));
            Assert.That(identity.ProcessReverse(value), Is.EqualTo(value).Within(Tolerance));
        }
        
        [Test]
        public void UnDi_UnDi_Concatenation([Random(-100d, 100d, 20)] double value)
        {
            var pipe = new StringToDoubleConverterPipe(Format, CultureInfo.InvariantCulture);
            var identity = pipe.ReversePipe().Append(pipe.ForwardPipe());

            Assert.That(identity.Process(value), Is.EqualTo(value).Within(Tolerance));
        }
        
        [Test]
        public void UnDi_BiDi_Concatenation([Random(-100d, 100d, 20)] double value)
        {
            var pipe = new StringToDoubleConverterPipe(Format, CultureInfo.InvariantCulture);
            var lhsIdentity = pipe.Invert().ForwardPipe().Append(pipe);
            var rhsIdentity = pipe.Invert().Append(pipe.ForwardPipe());

            Assert.That(lhsIdentity.Process(value), Is.EqualTo(value).Within(Tolerance));
            Assert.That(rhsIdentity.Process(value), Is.EqualTo(value).Within(Tolerance));
        }
        
        [Test]
        public void CompositeBiDi([Random(-100d, 100d, 20)] double value)
        {
            var lhsPipe = new StringToDoubleConverterPipe(Format, CultureInfo.InvariantCulture);
            var rhsPipe = new StringToDoubleConverterPipe(Format, CultureInfo.InvariantCulture);
            var fwdComposite = lhsPipe.ReversePipe().CreateBiDiPipe(rhsPipe.ForwardPipe());
            var revComposite = lhsPipe.ForwardPipe().CreateBiDiPipe(rhsPipe.ReversePipe());
            var identity = fwdComposite.Append(revComposite);

            Assert.That(identity.ProcessForward(value), Is.EqualTo(value).Within(Tolerance));
            Assert.That(identity.ProcessReverse(value), Is.EqualTo(value).Within(Tolerance));
        }

        [TestCase(1)]
        [TestCase(1d)]
        public void IdentityPipe<T>(T value)
        {
            var pipe = new IdentityPipe<T>();

            Assert.That(pipe.ProcessForward(value), Is.EqualTo(value));
            Assert.That(pipe.ProcessReverse(value), Is.EqualTo(value));
        }
    }
    
}