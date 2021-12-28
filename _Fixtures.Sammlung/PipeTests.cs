using System.Globalization;
using NUnit.Framework;
using Sammlung.Pipes;
using Sammlung.Pipes.Conversion;

namespace _Fixtures.Sammlung
{
    [TestFixture]
    public class PipeTests
    {
        [Test]
        public void BiDi_BiDi_Concatenation([Random(-100d, 100d, 20)] double value)
        {
            var pipe = new DoubleConvertStringPipe("N10", CultureInfo.InvariantCulture);
            var invPipe = pipe.Invert();
            var identity = pipe.Concat(invPipe);
            
            Assert.AreEqual(value, identity.ProcessForward(value), 1e-9);
            Assert.AreEqual(value, identity.ProcessReverse(value), 1e-9);
        }
        
        [Test]
        public void UnDi_UnDi_Concatenation([Random(-100d, 100d, 20)] double value)
        {
            var pipe = new DoubleConvertStringPipe("N10", CultureInfo.InvariantCulture);
            var invPipe = pipe.Invert();
            var identity = pipe.ForwardPipe().Concat(invPipe.ForwardPipe());
            
            Assert.AreEqual(value, identity.Process(value), 1e-9);
        }
        
        [Test]
        public void UnDi_BiDi_Concatenation([Random(-100d, 100d, 20)] double value)
        {
            var pipe = new DoubleConvertStringPipe("N16", CultureInfo.InvariantCulture);
            var invPipe = pipe.Invert();
            var lhsIdentity = pipe.Concat(invPipe.ForwardPipe());
            var rhsIdentity = pipe.ForwardPipe().Concat(invPipe);
            
            Assert.AreEqual(value, lhsIdentity.Process(value), 1e-15);
            Assert.AreEqual(value, rhsIdentity.Process(value), 1e-15);
        }
        
        [Test]
        public void CompositeBiDi([Random(-100d, 100d, 20)] double value)
        {
            var lhsPipe = new DoubleConvertStringPipe("N16", CultureInfo.InvariantCulture);
            var rhsPipe = new DoubleConvertStringPipe("N16", CultureInfo.InvariantCulture);
            var fwdComposite = lhsPipe.ForwardPipe().Combine(rhsPipe.ReversePipe());
            var revComposite = lhsPipe.ReversePipe().Combine(rhsPipe.ForwardPipe());
            var identity = fwdComposite.Concat(revComposite);
            
            Assert.AreEqual(value, identity.ProcessForward(value), 1e-15);
            Assert.AreEqual(value, identity.ProcessReverse(value), 1e-15);
        }
    }
}