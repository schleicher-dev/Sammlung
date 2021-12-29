using System.Globalization;
using NUnit.Framework;
using Sammlung.Pipes;
using Sammlung.Pipes.Werkzeug.Converters;

namespace _Fixtures.Sammlung
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class PipeTests
    {
        private const string Format = "N16";
        private const double Tolerance = 1e-12;
        
        [Test]
        public void BiDi_BiDi_Concatenation([Random(-100d, 100d, 20)] double value)
        {
            var pipe = new DoubleConvertStringPipe(Format, CultureInfo.InvariantCulture);
            var invPipe = pipe.Invert();
            var identity = pipe.Concat(invPipe);
            
            Assert.AreEqual(value, identity.ProcessForward(value), Tolerance);
            Assert.AreEqual(value, identity.ProcessReverse(value), Tolerance);
        }
        
        [Test]
        public void UnDi_UnDi_Concatenation([Random(-100d, 100d, 20)] double value)
        {
            var pipe = new DoubleConvertStringPipe(Format, CultureInfo.InvariantCulture);
            var invPipe = pipe.Invert();
            var identity = pipe.ForwardPipe().Concat(invPipe.ForwardPipe());
            
            Assert.AreEqual(value, identity.Process(value), Tolerance);
        }
        
        [Test]
        public void UnDi_BiDi_Concatenation([Random(-100d, 100d, 20)] double value)
        {
            var pipe = new DoubleConvertStringPipe(Format, CultureInfo.InvariantCulture);
            var invPipe = pipe.Invert();
            var lhsIdentity = pipe.Concat(invPipe.ForwardPipe());
            var rhsIdentity = pipe.ForwardPipe().Concat(invPipe);
            
            Assert.AreEqual(value, lhsIdentity.Process(value), Tolerance);
            Assert.AreEqual(value, rhsIdentity.Process(value), Tolerance);
        }
        
        [Test]
        public void CompositeBiDi([Random(-100d, 100d, 20)] double value)
        {
            var lhsPipe = new DoubleConvertStringPipe(Format, CultureInfo.InvariantCulture);
            var rhsPipe = new DoubleConvertStringPipe(Format, CultureInfo.InvariantCulture);
            var fwdComposite = lhsPipe.ForwardPipe().CreateBiDiPipe(rhsPipe.ReversePipe());
            var revComposite = lhsPipe.ReversePipe().CreateBiDiPipe(rhsPipe.ForwardPipe());
            var identity = fwdComposite.Concat(revComposite);
            
            Assert.AreEqual(value, identity.ProcessForward(value), Tolerance);
            Assert.AreEqual(value, identity.ProcessReverse(value), Tolerance);
        }
    }
}