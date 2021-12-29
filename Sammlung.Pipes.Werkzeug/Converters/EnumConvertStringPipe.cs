using System;

namespace Sammlung.Pipes.Werkzeug.Converters
{    /// <summary>
    /// The <see cref="EnumConvertStringPipe{T}" /> pipe converts a Enum to a string and vice-versa.
    /// </summary>
    public class EnumConvertStringPipe<T> : IBiDiPipe<T, string> where T : Enum
    {
        /// <inheritdoc />
        public string ProcessForward(T input) => Enum.GetName(typeof(T), input);

        /// <inheritdoc />
        public T ProcessReverse(string input) => (T) Enum.Parse(typeof(T), input);
    }
}