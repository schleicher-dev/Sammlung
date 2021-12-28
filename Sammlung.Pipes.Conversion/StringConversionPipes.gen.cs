
using System;
using System.Globalization;

namespace Sammlung.Pipes.Conversion {
    /// <summary>
    /// The <see cref="BooleanConvertStringPipe" /> pipe converts a bool to a string and vice-versa using the
    /// <see cref="IFormatProvider" /> passed into the constructor.
    /// </summary>
    public class BooleanConvertStringPipe : IBiDiPipe<bool, string> {
        private readonly IFormatProvider _formatProvider;
        
        /// <summary>
        /// Creates a new <see cref="BooleanConvertStringPipe" /> using a <see cref="IFormatProvider" />.
        /// When not set the <see cref="CultureInfo.InvariantCulture" /> is used.
        /// </summary>
        public BooleanConvertStringPipe(IFormatProvider formatProvider = null)
        {
            _formatProvider = formatProvider ?? CultureInfo.InvariantCulture;
        }

        /// <inheritdoc />
        public string ProcessForward(bool input)
            => input.ToString(_formatProvider);

        /// <inheritdoc />
        public bool ProcessReverse(string input)
            => bool.Parse(input);
    }

    /// <summary>
    /// The <see cref="CharConvertStringPipe" /> pipe converts a char to a string and vice-versa using the
    /// <see cref="IFormatProvider" /> passed into the constructor.
    /// </summary>
    public class CharConvertStringPipe : IBiDiPipe<char, string> {
        private readonly IFormatProvider _formatProvider;
        
        /// <summary>
        /// Creates a new <see cref="CharConvertStringPipe" /> using a <see cref="IFormatProvider" />.
        /// When not set the <see cref="CultureInfo.InvariantCulture" /> is used.
        /// </summary>
        public CharConvertStringPipe(IFormatProvider formatProvider = null)
        {
            _formatProvider = formatProvider ?? CultureInfo.InvariantCulture;
        }

        /// <inheritdoc />
        public string ProcessForward(char input)
            => input.ToString(_formatProvider);

        /// <inheritdoc />
        public char ProcessReverse(string input)
            => char.Parse(input);
    }

    /// <summary>
    /// The <see cref="DecimalConvertStringPipe" /> pipe converts a decimal to a string and vice-versa using the
    /// <see cref="IFormatProvider" /> passed into the constructor.
    /// </summary>
    public class DecimalConvertStringPipe : IBiDiPipe<decimal, string> {
        private readonly string _formatString;
        private readonly IFormatProvider _formatProvider;
        
        /// <summary>
        /// Creates a new <see cref="DecimalConvertStringPipe" /> using a <see cref="IFormatProvider" />.
        /// When not set the <see cref="CultureInfo.InvariantCulture" /> is used.
        /// </summary>
        public DecimalConvertStringPipe(string formatString = null, IFormatProvider formatProvider = null)
        {
            _formatString = formatString;
            _formatProvider = formatProvider ?? CultureInfo.InvariantCulture;
        }

        /// <inheritdoc />
        public string ProcessForward(decimal input)
            => input.ToString(_formatString, _formatProvider);

        /// <inheritdoc />
        public decimal ProcessReverse(string input)
            => decimal.Parse(input, NumberStyles.Any, _formatProvider);
    }

    /// <summary>
    /// The <see cref="DoubleConvertStringPipe" /> pipe converts a double to a string and vice-versa using the
    /// <see cref="IFormatProvider" /> passed into the constructor.
    /// </summary>
    public class DoubleConvertStringPipe : IBiDiPipe<double, string> {
        private readonly string _formatString;
        private readonly IFormatProvider _formatProvider;
        
        /// <summary>
        /// Creates a new <see cref="DoubleConvertStringPipe" /> using a <see cref="IFormatProvider" />.
        /// When not set the <see cref="CultureInfo.InvariantCulture" /> is used.
        /// </summary>
        public DoubleConvertStringPipe(string formatString = null, IFormatProvider formatProvider = null)
        {
            _formatString = formatString;
            _formatProvider = formatProvider ?? CultureInfo.InvariantCulture;
        }

        /// <inheritdoc />
        public string ProcessForward(double input)
            => input.ToString(_formatString, _formatProvider);

        /// <inheritdoc />
        public double ProcessReverse(string input)
            => double.Parse(input, NumberStyles.Any, _formatProvider);
    }

    /// <summary>
    /// The <see cref="SingleConvertStringPipe" /> pipe converts a float to a string and vice-versa using the
    /// <see cref="IFormatProvider" /> passed into the constructor.
    /// </summary>
    public class SingleConvertStringPipe : IBiDiPipe<float, string> {
        private readonly string _formatString;
        private readonly IFormatProvider _formatProvider;
        
        /// <summary>
        /// Creates a new <see cref="SingleConvertStringPipe" /> using a <see cref="IFormatProvider" />.
        /// When not set the <see cref="CultureInfo.InvariantCulture" /> is used.
        /// </summary>
        public SingleConvertStringPipe(string formatString = null, IFormatProvider formatProvider = null)
        {
            _formatString = formatString;
            _formatProvider = formatProvider ?? CultureInfo.InvariantCulture;
        }

        /// <inheritdoc />
        public string ProcessForward(float input)
            => input.ToString(_formatString, _formatProvider);

        /// <inheritdoc />
        public float ProcessReverse(string input)
            => float.Parse(input, NumberStyles.Any, _formatProvider);
    }

    /// <summary>
    /// The <see cref="Int32ConvertStringPipe" /> pipe converts a int to a string and vice-versa using the
    /// <see cref="IFormatProvider" /> passed into the constructor.
    /// </summary>
    public class Int32ConvertStringPipe : IBiDiPipe<int, string> {
        private readonly string _formatString;
        private readonly IFormatProvider _formatProvider;
        
        /// <summary>
        /// Creates a new <see cref="Int32ConvertStringPipe" /> using a <see cref="IFormatProvider" />.
        /// When not set the <see cref="CultureInfo.InvariantCulture" /> is used.
        /// </summary>
        public Int32ConvertStringPipe(string formatString = null, IFormatProvider formatProvider = null)
        {
            _formatString = formatString;
            _formatProvider = formatProvider ?? CultureInfo.InvariantCulture;
        }

        /// <inheritdoc />
        public string ProcessForward(int input)
            => input.ToString(_formatString, _formatProvider);

        /// <inheritdoc />
        public int ProcessReverse(string input)
            => int.Parse(input, NumberStyles.Any, _formatProvider);
    }

    /// <summary>
    /// The <see cref="UInt32ConvertStringPipe" /> pipe converts a uint to a string and vice-versa using the
    /// <see cref="IFormatProvider" /> passed into the constructor.
    /// </summary>
    public class UInt32ConvertStringPipe : IBiDiPipe<uint, string> {
        private readonly string _formatString;
        private readonly IFormatProvider _formatProvider;
        
        /// <summary>
        /// Creates a new <see cref="UInt32ConvertStringPipe" /> using a <see cref="IFormatProvider" />.
        /// When not set the <see cref="CultureInfo.InvariantCulture" /> is used.
        /// </summary>
        public UInt32ConvertStringPipe(string formatString = null, IFormatProvider formatProvider = null)
        {
            _formatString = formatString;
            _formatProvider = formatProvider ?? CultureInfo.InvariantCulture;
        }

        /// <inheritdoc />
        public string ProcessForward(uint input)
            => input.ToString(_formatString, _formatProvider);

        /// <inheritdoc />
        public uint ProcessReverse(string input)
            => uint.Parse(input, NumberStyles.Any, _formatProvider);
    }

    /// <summary>
    /// The <see cref="Int64ConvertStringPipe" /> pipe converts a long to a string and vice-versa using the
    /// <see cref="IFormatProvider" /> passed into the constructor.
    /// </summary>
    public class Int64ConvertStringPipe : IBiDiPipe<long, string> {
        private readonly string _formatString;
        private readonly IFormatProvider _formatProvider;
        
        /// <summary>
        /// Creates a new <see cref="Int64ConvertStringPipe" /> using a <see cref="IFormatProvider" />.
        /// When not set the <see cref="CultureInfo.InvariantCulture" /> is used.
        /// </summary>
        public Int64ConvertStringPipe(string formatString = null, IFormatProvider formatProvider = null)
        {
            _formatString = formatString;
            _formatProvider = formatProvider ?? CultureInfo.InvariantCulture;
        }

        /// <inheritdoc />
        public string ProcessForward(long input)
            => input.ToString(_formatString, _formatProvider);

        /// <inheritdoc />
        public long ProcessReverse(string input)
            => long.Parse(input, NumberStyles.Any, _formatProvider);
    }

    /// <summary>
    /// The <see cref="UInt64ConvertStringPipe" /> pipe converts a ulong to a string and vice-versa using the
    /// <see cref="IFormatProvider" /> passed into the constructor.
    /// </summary>
    public class UInt64ConvertStringPipe : IBiDiPipe<ulong, string> {
        private readonly string _formatString;
        private readonly IFormatProvider _formatProvider;
        
        /// <summary>
        /// Creates a new <see cref="UInt64ConvertStringPipe" /> using a <see cref="IFormatProvider" />.
        /// When not set the <see cref="CultureInfo.InvariantCulture" /> is used.
        /// </summary>
        public UInt64ConvertStringPipe(string formatString = null, IFormatProvider formatProvider = null)
        {
            _formatString = formatString;
            _formatProvider = formatProvider ?? CultureInfo.InvariantCulture;
        }

        /// <inheritdoc />
        public string ProcessForward(ulong input)
            => input.ToString(_formatString, _formatProvider);

        /// <inheritdoc />
        public ulong ProcessReverse(string input)
            => ulong.Parse(input, NumberStyles.Any, _formatProvider);
    }

    /// <summary>
    /// The <see cref="Int16ConvertStringPipe" /> pipe converts a short to a string and vice-versa using the
    /// <see cref="IFormatProvider" /> passed into the constructor.
    /// </summary>
    public class Int16ConvertStringPipe : IBiDiPipe<short, string> {
        private readonly string _formatString;
        private readonly IFormatProvider _formatProvider;
        
        /// <summary>
        /// Creates a new <see cref="Int16ConvertStringPipe" /> using a <see cref="IFormatProvider" />.
        /// When not set the <see cref="CultureInfo.InvariantCulture" /> is used.
        /// </summary>
        public Int16ConvertStringPipe(string formatString = null, IFormatProvider formatProvider = null)
        {
            _formatString = formatString;
            _formatProvider = formatProvider ?? CultureInfo.InvariantCulture;
        }

        /// <inheritdoc />
        public string ProcessForward(short input)
            => input.ToString(_formatString, _formatProvider);

        /// <inheritdoc />
        public short ProcessReverse(string input)
            => short.Parse(input, NumberStyles.Any, _formatProvider);
    }

    /// <summary>
    /// The <see cref="UInt16ConvertStringPipe" /> pipe converts a ushort to a string and vice-versa using the
    /// <see cref="IFormatProvider" /> passed into the constructor.
    /// </summary>
    public class UInt16ConvertStringPipe : IBiDiPipe<ushort, string> {
        private readonly string _formatString;
        private readonly IFormatProvider _formatProvider;
        
        /// <summary>
        /// Creates a new <see cref="UInt16ConvertStringPipe" /> using a <see cref="IFormatProvider" />.
        /// When not set the <see cref="CultureInfo.InvariantCulture" /> is used.
        /// </summary>
        public UInt16ConvertStringPipe(string formatString = null, IFormatProvider formatProvider = null)
        {
            _formatString = formatString;
            _formatProvider = formatProvider ?? CultureInfo.InvariantCulture;
        }

        /// <inheritdoc />
        public string ProcessForward(ushort input)
            => input.ToString(_formatString, _formatProvider);

        /// <inheritdoc />
        public ushort ProcessReverse(string input)
            => ushort.Parse(input, NumberStyles.Any, _formatProvider);
    }

}
