using System;
using System.Globalization;
using NUnit.Framework;
using Sammlung.Pipes;
using Sammlung.Pipes.Conversion;

namespace _Fixtures.Sammlung
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class ConversionPipeTests
    {
        private const string GermanCulture = "de-de";
        
        private const string IntWithSignFormat = "+#;-#;0";
        private const string IntWithGroupFormat = "N0";
        private const string FloatWithSignFormat = "+#.##;-#.##;0";
        private const string FloatWithGroupFormat = "N";

        private const double UpperFloatTestValue = 1234.25f;
        private const double LowerFloatTestValue = -1234.25f;

        private static void AssertResults<T>(IBiDiPipe<T, string> pipe, T input, string output)
        {
            Assert.AreEqual(output, pipe.ProcessForward(input));
            Assert.AreEqual(input, pipe.ProcessReverse(output));
        }
        
        [TestCase(null, null, 0, "0")]
        [TestCase(null, null, UpperFloatTestValue, "1234.25")]
        [TestCase(null, null, LowerFloatTestValue, "-1234.25")]
        [TestCase(null, FloatWithSignFormat, 0, "0")]
        [TestCase(null, FloatWithSignFormat, UpperFloatTestValue, "+1234.25")]
        [TestCase(null, FloatWithSignFormat, LowerFloatTestValue, "-1234.25")]
        [TestCase(null, FloatWithGroupFormat, 0, "0.00")]
        [TestCase(null, FloatWithGroupFormat, UpperFloatTestValue, "1,234.25")]
        [TestCase(null, FloatWithGroupFormat, LowerFloatTestValue, "-1,234.25")]
        [TestCase(GermanCulture, null, 0, "0")]
        [TestCase(GermanCulture, null, UpperFloatTestValue, "1234,25")]
        [TestCase(GermanCulture, null, LowerFloatTestValue, "-1234,25")]
        [TestCase(GermanCulture, FloatWithSignFormat, 0, "0")]
        [TestCase(GermanCulture, FloatWithSignFormat, UpperFloatTestValue, "+1234,25")]
        [TestCase(GermanCulture, FloatWithSignFormat, LowerFloatTestValue, "-1234,25")]
        [TestCase(GermanCulture, FloatWithGroupFormat, 0, "0,00")]
        [TestCase(GermanCulture, FloatWithGroupFormat, UpperFloatTestValue, "1.234,25")]
        [TestCase(GermanCulture, FloatWithGroupFormat, LowerFloatTestValue, "-1.234,25")]
        public void SingleAndDoubleAndDecimalToStringConversionTests(string cultureName, string format, double input, string output)
        {
            var formatProvider = cultureName == null
                ? CultureInfo.InvariantCulture
                : CultureInfo.GetCultureInfo(cultureName);
            var sglPipe = new SingleConvertStringPipe(format, formatProvider);
            var dblPipe = new DoubleConvertStringPipe(format, formatProvider);
            var dcmPipe = new DecimalConvertStringPipe(format, formatProvider);
            
            AssertResults(sglPipe, (float) input, output);
            AssertResults(dblPipe, input, output);
            AssertResults(dcmPipe, (decimal) input, output);
        }
        
        [TestCase(null, null, 0, "0")]
        [TestCase(null, null, short.MaxValue, "32767")]
        [TestCase(null, null, short.MinValue, "-32768")]
        [TestCase(null, IntWithSignFormat, 0, "0")]
        [TestCase(null, IntWithSignFormat, short.MaxValue, "+32767")]
        [TestCase(null, IntWithSignFormat, short.MinValue, "-32768")]
        [TestCase(null, IntWithGroupFormat, 0, "0")]
        [TestCase(null, IntWithGroupFormat, short.MaxValue, "32,767")]
        [TestCase(null, IntWithGroupFormat, short.MinValue, "-32,768")]
        [TestCase(GermanCulture, null, 0, "0")]
        [TestCase(GermanCulture, null, short.MaxValue, "32767")]
        [TestCase(GermanCulture, null, short.MinValue, "-32768")]
        [TestCase(GermanCulture, IntWithSignFormat, 0, "0")]
        [TestCase(GermanCulture, IntWithSignFormat, short.MaxValue, "+32767")]
        [TestCase(GermanCulture, IntWithSignFormat, short.MinValue, "-32768")]
        [TestCase(GermanCulture, IntWithGroupFormat, 0, "0")]
        [TestCase(GermanCulture, IntWithGroupFormat, short.MaxValue, "32.767")]
        [TestCase(GermanCulture, IntWithGroupFormat, short.MinValue, "-32.768")]
        public void Int16ToStringConversionTests(string cultureName, string format, short input, string output)
        {
            var formatProvider = cultureName == null
                ? CultureInfo.InvariantCulture
                : CultureInfo.GetCultureInfo(cultureName);
            var pipe = new Int16ConvertStringPipe(format, formatProvider);
            AssertResults(pipe, input, output);
        }

        [TestCase(null, null, ushort.MaxValue, "65535")]
        [TestCase(null, null, ushort.MinValue, "0")]
        [TestCase(null, IntWithSignFormat, ushort.MaxValue, "+65535")]
        [TestCase(null, IntWithSignFormat, ushort.MinValue, "0")]
        [TestCase(null, IntWithGroupFormat, ushort.MaxValue, "65,535")]
        [TestCase(null, IntWithGroupFormat, ushort.MinValue, "0")]
        [TestCase(GermanCulture, null, ushort.MaxValue, "65535")]
        [TestCase(GermanCulture, null, ushort.MinValue, "0")]
        [TestCase(GermanCulture, IntWithSignFormat, ushort.MaxValue, "+65535")]
        [TestCase(GermanCulture, IntWithSignFormat, ushort.MinValue, "0")]
        [TestCase(GermanCulture, IntWithGroupFormat, ushort.MaxValue, "65.535")]
        [TestCase(GermanCulture, IntWithGroupFormat, ushort.MinValue, "0")]
        public void UInt16ToStringConversionTests(string cultureName, string format, ushort input, string output)
        {
            var formatProvider = cultureName == null
                ? CultureInfo.InvariantCulture
                : CultureInfo.GetCultureInfo(cultureName);
            var pipe = new UInt16ConvertStringPipe(format, formatProvider);
            AssertResults(pipe, input, output);
        }
        
        [TestCase(null, null, 0, "0")]
        [TestCase(null, null, int.MaxValue, "2147483647")]
        [TestCase(null, null, int.MinValue, "-2147483648")]
        [TestCase(null, IntWithSignFormat, 0, "0")]
        [TestCase(null, IntWithSignFormat, int.MaxValue, "+2147483647")]
        [TestCase(null, IntWithSignFormat, int.MinValue, "-2147483648")]
        [TestCase(null, IntWithGroupFormat, 0, "0")]
        [TestCase(null, IntWithGroupFormat, int.MaxValue, "2,147,483,647")]
        [TestCase(null, IntWithGroupFormat, int.MinValue, "-2,147,483,648")]
        [TestCase(GermanCulture, null, 0, "0")]
        [TestCase(GermanCulture, null, int.MaxValue, "2147483647")]
        [TestCase(GermanCulture, null, int.MinValue, "-2147483648")]
        [TestCase(GermanCulture, IntWithSignFormat, 0, "0")]
        [TestCase(GermanCulture, IntWithSignFormat, int.MaxValue, "+2147483647")]
        [TestCase(GermanCulture, IntWithSignFormat, int.MinValue, "-2147483648")]
        [TestCase(GermanCulture, IntWithGroupFormat, 0, "0")]
        [TestCase(GermanCulture, IntWithGroupFormat, int.MaxValue, "2.147.483.647")]
        [TestCase(GermanCulture, IntWithGroupFormat, int.MinValue, "-2.147.483.648")]
        public void Int32ToStringConversionTests(string cultureName, string format, int input, string output)
        {
            var formatProvider = cultureName == null
                ? CultureInfo.InvariantCulture
                : CultureInfo.GetCultureInfo(cultureName);
            var pipe = new Int32ConvertStringPipe(format, formatProvider);
            AssertResults(pipe, input, output);
        }

        [TestCase(null, null, uint.MaxValue, "4294967295")]
        [TestCase(null, null, uint.MinValue, "0")]
        [TestCase(null, IntWithSignFormat, uint.MaxValue, "+4294967295")]
        [TestCase(null, IntWithSignFormat, uint.MinValue, "0")]
        [TestCase(null, IntWithGroupFormat, uint.MaxValue, "4,294,967,295")]
        [TestCase(null, IntWithGroupFormat, uint.MinValue, "0")]
        [TestCase(GermanCulture, null, uint.MaxValue, "4294967295")]
        [TestCase(GermanCulture, null, uint.MinValue, "0")]
        [TestCase(GermanCulture, IntWithSignFormat, uint.MaxValue, "+4294967295")]
        [TestCase(GermanCulture, IntWithSignFormat, uint.MinValue, "0")]
        [TestCase(GermanCulture, IntWithGroupFormat, uint.MaxValue, "4.294.967.295")]
        [TestCase(GermanCulture, IntWithGroupFormat, uint.MinValue, "0")]
        public void UInt32ToStringConversionTests(string cultureName, string format, uint input, string output)
        {
            var formatProvider = cultureName == null
                ? CultureInfo.InvariantCulture
                : CultureInfo.GetCultureInfo(cultureName);
            var pipe = new UInt32ConvertStringPipe(format, formatProvider);
            AssertResults(pipe, input, output);
        }
        
        [TestCase(null, null, 0, "0")]
        [TestCase(null, null, long.MaxValue, "9223372036854775807")]
        [TestCase(null, null, long.MinValue, "-9223372036854775808")]
        [TestCase(null, IntWithSignFormat, 0, "0")]
        [TestCase(null, IntWithSignFormat, long.MaxValue, "+9223372036854775807")]
        [TestCase(null, IntWithSignFormat, long.MinValue, "-9223372036854775808")]
        [TestCase(null, IntWithGroupFormat, 0, "0")]
        [TestCase(null, IntWithGroupFormat, long.MaxValue, "9,223,372,036,854,775,807")]
        [TestCase(null, IntWithGroupFormat, long.MinValue, "-9,223,372,036,854,775,808")]
        [TestCase(GermanCulture, null, 0, "0")]
        [TestCase(GermanCulture, null, long.MaxValue, "9223372036854775807")]
        [TestCase(GermanCulture, null, long.MinValue, "-9223372036854775808")]
        [TestCase(GermanCulture, IntWithSignFormat, 0, "0")]
        [TestCase(GermanCulture, IntWithSignFormat, long.MaxValue, "+9223372036854775807")]
        [TestCase(GermanCulture, IntWithSignFormat, long.MinValue, "-9223372036854775808")]
        [TestCase(GermanCulture, IntWithGroupFormat, 0, "0")]
        [TestCase(GermanCulture, IntWithGroupFormat, long.MaxValue, "9.223.372.036.854.775.807")]
        [TestCase(GermanCulture, IntWithGroupFormat, long.MinValue, "-9.223.372.036.854.775.808")]
        public void Int64ToStringConversionTests(string cultureName, string format, long input, string output)
        {
            var formatProvider = cultureName == null
                ? CultureInfo.InvariantCulture
                : CultureInfo.GetCultureInfo(cultureName);
            var pipe = new Int64ConvertStringPipe(format, formatProvider);
            AssertResults(pipe, input, output);
        }

        [TestCase(null, null, ulong.MaxValue, "18446744073709551615")]
        [TestCase(null, null, ulong.MinValue, "0")]
        [TestCase(null, IntWithSignFormat, ulong.MaxValue, "+18446744073709551615")]
        [TestCase(null, IntWithSignFormat, ulong.MinValue, "0")]
        [TestCase(null, IntWithGroupFormat, ulong.MaxValue, "18,446,744,073,709,551,615")]
        [TestCase(null, IntWithGroupFormat, ulong.MinValue, "0")]
        [TestCase(GermanCulture, null, ulong.MaxValue, "18446744073709551615")]
        [TestCase(GermanCulture, null, ulong.MinValue, "0")]
        [TestCase(GermanCulture, IntWithSignFormat, ulong.MaxValue, "+18446744073709551615")]
        [TestCase(GermanCulture, IntWithSignFormat, ulong.MinValue, "0")]
        [TestCase(GermanCulture, IntWithGroupFormat, ulong.MaxValue, "18.446.744.073.709.551.615")]
        [TestCase(GermanCulture, IntWithGroupFormat, ulong.MinValue, "0")]
        public void UInt64ToStringConversionTests(string cultureName, string format, ulong input, string output)
        {
            var formatProvider = cultureName == null
                ? CultureInfo.InvariantCulture
                : CultureInfo.GetCultureInfo(cultureName);
            var pipe = new UInt64ConvertStringPipe(format, formatProvider);
            AssertResults(pipe, input, output);
        }

        [Test]
        public void BooleanToStringConversionTests()
        {
            var pipe = new BooleanConvertStringPipe(CultureInfo.InvariantCulture);

            Assert.AreEqual("True", pipe.ProcessForward(true));
            Assert.AreEqual("False", pipe.ProcessForward(false));
            Assert.AreEqual(true, pipe.ProcessReverse("True"));
            Assert.AreEqual(false, pipe.ProcessReverse("False"));
            Assert.AreEqual(true, pipe.ProcessReverse(bool.TrueString));
            Assert.AreEqual(false, pipe.ProcessReverse(bool.FalseString));
        }

        [Test]
        public void CharToStringConversionTests()
        {
            var pipe = new CharConvertStringPipe(CultureInfo.InvariantCulture);
            
            Assert.AreEqual("a", pipe.ProcessForward('a'));
            Assert.AreEqual('a', pipe.ProcessReverse("a"));
            Assert.Throws<FormatException>(() => _ = pipe.ProcessReverse("abc"));
        }
    }
}