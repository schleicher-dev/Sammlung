using System;

namespace Sammlung.Pipes.Werkzeug.Converters
{
    /// <summary>
    /// The <see cref="StringToEnumConverterPipe{T}" /> pipe converts a Enum to a string and vice-versa.
    /// </summary>
    public class StringToEnumConverterPipe<T> : IBiDiPipe<string, T> where T : Enum
    {
        /// <inheritdoc />
        public T ProcessForward(string input) => (T)Enum.Parse(typeof(T), input);

        /// <inheritdoc />
        public string ProcessReverse(T input) => Enum.GetName(typeof(T), input);
    }
}