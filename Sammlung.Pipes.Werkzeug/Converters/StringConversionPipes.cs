
using System;
using System.Globalization;

namespace Sammlung.Pipes.Werkzeug.Converters {
    /// <summary>
    /// The <see cref="StringToBooleanConverterPipe" /> pipe converts a bool to a string and vice-versa using the
    /// <see cref="IFormatProvider" /> passed into the constructor.
    /// </summary>
    [JetBrains.Annotations.PublicAPI]
    public class StringToBooleanConverterPipe : IBiDiPipe<string, bool> {
        private readonly IFormatProvider _formatProvider;
        
        /// <summary>
        /// Creates a new <see cref="StringToBooleanConverterPipe" /> using a <see cref="IFormatProvider" />.
        /// When not set the <see cref="CultureInfo.InvariantCulture" /> is used.
        /// </summary>
        public StringToBooleanConverterPipe(IFormatProvider formatProvider = null)
        {
            _formatProvider = formatProvider ?? CultureInfo.InvariantCulture;
        }

        /// <inheritdoc />
        public bool ProcessForward(string input)
            => bool.Parse(input);

        /// <inheritdoc />
        public string ProcessReverse(bool input)
            => input.ToString(_formatProvider);
    }

    /// <summary>
    /// The <see cref="StringToCharConverterPipe" /> pipe converts a char to a string and vice-versa using the
    /// <see cref="IFormatProvider" /> passed into the constructor.
    /// </summary>
    [JetBrains.Annotations.PublicAPI]
    public class StringToCharConverterPipe : IBiDiPipe<string, char> {
        private readonly IFormatProvider _formatProvider;
        
        /// <summary>
        /// Creates a new <see cref="StringToCharConverterPipe" /> using a <see cref="IFormatProvider" />.
        /// When not set the <see cref="CultureInfo.InvariantCulture" /> is used.
        /// </summary>
        public StringToCharConverterPipe(IFormatProvider formatProvider = null)
        {
            _formatProvider = formatProvider ?? CultureInfo.InvariantCulture;
        }

        /// <inheritdoc />
        public char ProcessForward(string input)
            => char.Parse(input);

        /// <inheritdoc />
        public string ProcessReverse(char input)
            => input.ToString(_formatProvider);
    }

    /// <summary>
    /// The <see cref="StringToDecimalConverterPipe" /> pipe converts a decimal to a string and vice-versa using the
    /// <see cref="IFormatProvider" /> passed into the constructor.
    /// </summary>
    [JetBrains.Annotations.PublicAPI]
    public class StringToDecimalConverterPipe : IBiDiPipe<string, decimal> {
        private readonly string _formatString;
        private readonly IFormatProvider _formatProvider;
        
        /// <summary>
        /// Creates a new <see cref="StringToDecimalConverterPipe" /> using a <see cref="IFormatProvider" />.
        /// When not set the <see cref="CultureInfo.InvariantCulture" /> is used.
        /// </summary>
        public StringToDecimalConverterPipe(string formatString = null, IFormatProvider formatProvider = null)
        {
            _formatString = formatString;
            _formatProvider = formatProvider ?? CultureInfo.InvariantCulture;
        }

        /// <inheritdoc />
        public decimal ProcessForward(string input)
            => decimal.Parse(input, NumberStyles.Any, _formatProvider);

        /// <inheritdoc />
        public string ProcessReverse(decimal input)
            => input.ToString(_formatString, _formatProvider);
    }

    /// <summary>
    /// The <see cref="StringToDoubleConverterPipe" /> pipe converts a double to a string and vice-versa using the
    /// <see cref="IFormatProvider" /> passed into the constructor.
    /// </summary>
    [JetBrains.Annotations.PublicAPI]
    public class StringToDoubleConverterPipe : IBiDiPipe<string, double> {
        private readonly string _formatString;
        private readonly IFormatProvider _formatProvider;
        
        /// <summary>
        /// Creates a new <see cref="StringToDoubleConverterPipe" /> using a <see cref="IFormatProvider" />.
        /// When not set the <see cref="CultureInfo.InvariantCulture" /> is used.
        /// </summary>
        public StringToDoubleConverterPipe(string formatString = null, IFormatProvider formatProvider = null)
        {
            _formatString = formatString;
            _formatProvider = formatProvider ?? CultureInfo.InvariantCulture;
        }

        /// <inheritdoc />
        public double ProcessForward(string input)
            => double.Parse(input, NumberStyles.Any, _formatProvider);

        /// <inheritdoc />
        public string ProcessReverse(double input)
            => input.ToString(_formatString, _formatProvider);
    }

    /// <summary>
    /// The <see cref="StringToSingleConverterPipe" /> pipe converts a float to a string and vice-versa using the
    /// <see cref="IFormatProvider" /> passed into the constructor.
    /// </summary>
    [JetBrains.Annotations.PublicAPI]
    public class StringToSingleConverterPipe : IBiDiPipe<string, float> {
        private readonly string _formatString;
        private readonly IFormatProvider _formatProvider;
        
        /// <summary>
        /// Creates a new <see cref="StringToSingleConverterPipe" /> using a <see cref="IFormatProvider" />.
        /// When not set the <see cref="CultureInfo.InvariantCulture" /> is used.
        /// </summary>
        public StringToSingleConverterPipe(string formatString = null, IFormatProvider formatProvider = null)
        {
            _formatString = formatString;
            _formatProvider = formatProvider ?? CultureInfo.InvariantCulture;
        }

        /// <inheritdoc />
        public float ProcessForward(string input)
            => float.Parse(input, NumberStyles.Any, _formatProvider);

        /// <inheritdoc />
        public string ProcessReverse(float input)
            => input.ToString(_formatString, _formatProvider);
    }

    /// <summary>
    /// The <see cref="StringToSByteConverterPipe" /> pipe converts a sbyte to a string and vice-versa using the
    /// <see cref="IFormatProvider" /> passed into the constructor.
    /// </summary>
    [JetBrains.Annotations.PublicAPI]
    public class StringToSByteConverterPipe : IBiDiPipe<string, sbyte> {
        private readonly string _formatString;
        private readonly IFormatProvider _formatProvider;
        
        /// <summary>
        /// Creates a new <see cref="StringToSByteConverterPipe" /> using a <see cref="IFormatProvider" />.
        /// When not set the <see cref="CultureInfo.InvariantCulture" /> is used.
        /// </summary>
        public StringToSByteConverterPipe(string formatString = null, IFormatProvider formatProvider = null)
        {
            _formatString = formatString;
            _formatProvider = formatProvider ?? CultureInfo.InvariantCulture;
        }

        /// <inheritdoc />
        public sbyte ProcessForward(string input)
            => sbyte.Parse(input, NumberStyles.Any, _formatProvider);

        /// <inheritdoc />
        public string ProcessReverse(sbyte input)
            => input.ToString(_formatString, _formatProvider);
    }

    /// <summary>
    /// The <see cref="StringToByteConverterPipe" /> pipe converts a byte to a string and vice-versa using the
    /// <see cref="IFormatProvider" /> passed into the constructor.
    /// </summary>
    [JetBrains.Annotations.PublicAPI]
    public class StringToByteConverterPipe : IBiDiPipe<string, byte> {
        private readonly string _formatString;
        private readonly IFormatProvider _formatProvider;
        
        /// <summary>
        /// Creates a new <see cref="StringToByteConverterPipe" /> using a <see cref="IFormatProvider" />.
        /// When not set the <see cref="CultureInfo.InvariantCulture" /> is used.
        /// </summary>
        public StringToByteConverterPipe(string formatString = null, IFormatProvider formatProvider = null)
        {
            _formatString = formatString;
            _formatProvider = formatProvider ?? CultureInfo.InvariantCulture;
        }

        /// <inheritdoc />
        public byte ProcessForward(string input)
            => byte.Parse(input, NumberStyles.Any, _formatProvider);

        /// <inheritdoc />
        public string ProcessReverse(byte input)
            => input.ToString(_formatString, _formatProvider);
    }

    /// <summary>
    /// The <see cref="StringToInt16ConverterPipe" /> pipe converts a short to a string and vice-versa using the
    /// <see cref="IFormatProvider" /> passed into the constructor.
    /// </summary>
    [JetBrains.Annotations.PublicAPI]
    public class StringToInt16ConverterPipe : IBiDiPipe<string, short> {
        private readonly string _formatString;
        private readonly IFormatProvider _formatProvider;
        
        /// <summary>
        /// Creates a new <see cref="StringToInt16ConverterPipe" /> using a <see cref="IFormatProvider" />.
        /// When not set the <see cref="CultureInfo.InvariantCulture" /> is used.
        /// </summary>
        public StringToInt16ConverterPipe(string formatString = null, IFormatProvider formatProvider = null)
        {
            _formatString = formatString;
            _formatProvider = formatProvider ?? CultureInfo.InvariantCulture;
        }

        /// <inheritdoc />
        public short ProcessForward(string input)
            => short.Parse(input, NumberStyles.Any, _formatProvider);

        /// <inheritdoc />
        public string ProcessReverse(short input)
            => input.ToString(_formatString, _formatProvider);
    }

    /// <summary>
    /// The <see cref="StringToUInt16ConverterPipe" /> pipe converts a ushort to a string and vice-versa using the
    /// <see cref="IFormatProvider" /> passed into the constructor.
    /// </summary>
    [JetBrains.Annotations.PublicAPI]
    public class StringToUInt16ConverterPipe : IBiDiPipe<string, ushort> {
        private readonly string _formatString;
        private readonly IFormatProvider _formatProvider;
        
        /// <summary>
        /// Creates a new <see cref="StringToUInt16ConverterPipe" /> using a <see cref="IFormatProvider" />.
        /// When not set the <see cref="CultureInfo.InvariantCulture" /> is used.
        /// </summary>
        public StringToUInt16ConverterPipe(string formatString = null, IFormatProvider formatProvider = null)
        {
            _formatString = formatString;
            _formatProvider = formatProvider ?? CultureInfo.InvariantCulture;
        }

        /// <inheritdoc />
        public ushort ProcessForward(string input)
            => ushort.Parse(input, NumberStyles.Any, _formatProvider);

        /// <inheritdoc />
        public string ProcessReverse(ushort input)
            => input.ToString(_formatString, _formatProvider);
    }

    /// <summary>
    /// The <see cref="StringToInt32ConverterPipe" /> pipe converts a int to a string and vice-versa using the
    /// <see cref="IFormatProvider" /> passed into the constructor.
    /// </summary>
    [JetBrains.Annotations.PublicAPI]
    public class StringToInt32ConverterPipe : IBiDiPipe<string, int> {
        private readonly string _formatString;
        private readonly IFormatProvider _formatProvider;
        
        /// <summary>
        /// Creates a new <see cref="StringToInt32ConverterPipe" /> using a <see cref="IFormatProvider" />.
        /// When not set the <see cref="CultureInfo.InvariantCulture" /> is used.
        /// </summary>
        public StringToInt32ConverterPipe(string formatString = null, IFormatProvider formatProvider = null)
        {
            _formatString = formatString;
            _formatProvider = formatProvider ?? CultureInfo.InvariantCulture;
        }

        /// <inheritdoc />
        public int ProcessForward(string input)
            => int.Parse(input, NumberStyles.Any, _formatProvider);

        /// <inheritdoc />
        public string ProcessReverse(int input)
            => input.ToString(_formatString, _formatProvider);
    }

    /// <summary>
    /// The <see cref="StringToUInt32ConverterPipe" /> pipe converts a uint to a string and vice-versa using the
    /// <see cref="IFormatProvider" /> passed into the constructor.
    /// </summary>
    [JetBrains.Annotations.PublicAPI]
    public class StringToUInt32ConverterPipe : IBiDiPipe<string, uint> {
        private readonly string _formatString;
        private readonly IFormatProvider _formatProvider;
        
        /// <summary>
        /// Creates a new <see cref="StringToUInt32ConverterPipe" /> using a <see cref="IFormatProvider" />.
        /// When not set the <see cref="CultureInfo.InvariantCulture" /> is used.
        /// </summary>
        public StringToUInt32ConverterPipe(string formatString = null, IFormatProvider formatProvider = null)
        {
            _formatString = formatString;
            _formatProvider = formatProvider ?? CultureInfo.InvariantCulture;
        }

        /// <inheritdoc />
        public uint ProcessForward(string input)
            => uint.Parse(input, NumberStyles.Any, _formatProvider);

        /// <inheritdoc />
        public string ProcessReverse(uint input)
            => input.ToString(_formatString, _formatProvider);
    }

    /// <summary>
    /// The <see cref="StringToInt64ConverterPipe" /> pipe converts a long to a string and vice-versa using the
    /// <see cref="IFormatProvider" /> passed into the constructor.
    /// </summary>
    [JetBrains.Annotations.PublicAPI]
    public class StringToInt64ConverterPipe : IBiDiPipe<string, long> {
        private readonly string _formatString;
        private readonly IFormatProvider _formatProvider;
        
        /// <summary>
        /// Creates a new <see cref="StringToInt64ConverterPipe" /> using a <see cref="IFormatProvider" />.
        /// When not set the <see cref="CultureInfo.InvariantCulture" /> is used.
        /// </summary>
        public StringToInt64ConverterPipe(string formatString = null, IFormatProvider formatProvider = null)
        {
            _formatString = formatString;
            _formatProvider = formatProvider ?? CultureInfo.InvariantCulture;
        }

        /// <inheritdoc />
        public long ProcessForward(string input)
            => long.Parse(input, NumberStyles.Any, _formatProvider);

        /// <inheritdoc />
        public string ProcessReverse(long input)
            => input.ToString(_formatString, _formatProvider);
    }

    /// <summary>
    /// The <see cref="StringToUInt64ConverterPipe" /> pipe converts a ulong to a string and vice-versa using the
    /// <see cref="IFormatProvider" /> passed into the constructor.
    /// </summary>
    [JetBrains.Annotations.PublicAPI]
    public class StringToUInt64ConverterPipe : IBiDiPipe<string, ulong> {
        private readonly string _formatString;
        private readonly IFormatProvider _formatProvider;
        
        /// <summary>
        /// Creates a new <see cref="StringToUInt64ConverterPipe" /> using a <see cref="IFormatProvider" />.
        /// When not set the <see cref="CultureInfo.InvariantCulture" /> is used.
        /// </summary>
        public StringToUInt64ConverterPipe(string formatString = null, IFormatProvider formatProvider = null)
        {
            _formatString = formatString;
            _formatProvider = formatProvider ?? CultureInfo.InvariantCulture;
        }

        /// <inheritdoc />
        public ulong ProcessForward(string input)
            => ulong.Parse(input, NumberStyles.Any, _formatProvider);

        /// <inheritdoc />
        public string ProcessReverse(ulong input)
            => input.ToString(_formatString, _formatProvider);
    }

}
