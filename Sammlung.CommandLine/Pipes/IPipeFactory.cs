using System;
using Sammlung.Pipes;

namespace Sammlung.CommandLine.Pipes
{
    /// <summary>
    /// The <see cref="IPipeFactory"/> yields all the standard pipes in a fluent fashion.
    /// </summary>
    [JetBrains.Annotations.PublicAPI]
    public interface IPipeFactory
    {
        /// <summary>
        /// The <see cref="BoolPipe"/> creates a pipe which converts a string to a boolean.
        /// </summary>
        /// <param name="provider">the format provider for the converter</param>
        /// <returns>the pipe</returns>
        IUnDiPipe<string, bool> BoolPipe(IFormatProvider provider = null);

        /// <summary>
        /// The <see cref="SinglePipe"/> creates a pipe which converts a string to a single.
        /// </summary>
        /// <param name="fmtStr">the format string</param>
        /// <param name="provider">the format provider for the converter</param>
        /// <returns>the pipe</returns>
        IUnDiPipe<string, float> SinglePipe(string fmtStr = null, IFormatProvider provider = null);
        
        /// <summary>
        /// The <see cref="DoublePipe"/> creates a pipe which converts a string to a double.
        /// </summary>
        /// <param name="fmtStr">the format string</param>
        /// <param name="provider">the format provider for the converter</param>
        /// <returns>the pipe</returns>
        IUnDiPipe<string, double> DoublePipe(string fmtStr = null, IFormatProvider provider = null);
        
        /// <summary>
        /// The <see cref="DecimalPipe"/> creates a pipe which converts a string to a decimal.
        /// </summary>
        /// <param name="fmtStr">the format string</param>
        /// <param name="provider">the format provider for the converter</param>
        /// <returns>the pipe</returns>
        IUnDiPipe<string, decimal> DecimalPipe(string fmtStr = null, IFormatProvider provider = null);
        
        /// <summary>
        /// The <see cref="CharPipe"/> creates a pipe which converts a string to a char.
        /// </summary>
        /// <param name="fmtStr">the format string</param>
        /// <returns>the pipe</returns>
        IUnDiPipe<string, char> CharPipe(IFormatProvider fmtStr = null);
        
        /// <summary>
        /// The <see cref="SBytePipe"/> creates a pipe which converts a string to a sbyte.
        /// </summary>
        /// <param name="fmtStr">the format string</param>
        /// <param name="provider">the format provider for the converter</param>
        /// <returns>the pipe</returns>
        IUnDiPipe<string, sbyte> SBytePipe(string fmtStr = null, IFormatProvider provider = null);
        
        /// <summary>
        /// The <see cref="BytePipe"/> creates a pipe which converts a string to a byte.
        /// </summary>
        /// <param name="fmtStr">the format string</param>
        /// <param name="provider">the format provider for the converter</param>
        /// <returns>the pipe</returns>
        IUnDiPipe<string, byte> BytePipe(string fmtStr = null, IFormatProvider provider = null);
        
        /// <summary>
        /// The <see cref="Int16Pipe"/> creates a pipe which converts a string to a short.
        /// </summary>
        /// <param name="fmtStr">the format string</param>
        /// <param name="provider">the format provider for the converter</param>
        /// <returns>the pipe</returns>
        IUnDiPipe<string, short> Int16Pipe(string fmtStr = null, IFormatProvider provider = null);
        
        /// <summary>
        /// The <see cref="UInt16Pipe"/> creates a pipe which converts a string to an ushort.
        /// </summary>
        /// <param name="fmtStr">the format string</param>
        /// <param name="provider">the format provider for the converter</param>
        /// <returns>the pipe</returns>
        IUnDiPipe<string, ushort> UInt16Pipe(string fmtStr = null, IFormatProvider provider = null);
        
        /// <summary>
        /// The <see cref="Int32Pipe"/> creates a pipe which converts a string to an integer.
        /// </summary>
        /// <param name="fmtStr">the format string</param>
        /// <param name="provider">the format provider for the converter</param>
        /// <returns>the pipe</returns>
        IUnDiPipe<string, int> Int32Pipe(string fmtStr = null, IFormatProvider provider = null);
        
        /// <summary>
        /// The <see cref="UInt32Pipe"/> creates a pipe which converts a string to an uint.
        /// </summary>
        /// <param name="fmtStr">the format string</param>
        /// <param name="provider">the format provider for the converter</param>
        /// <returns>the pipe</returns>
        IUnDiPipe<string, uint> UInt32Pipe(string fmtStr = null, IFormatProvider provider = null);
        
        /// <summary>
        /// The <see cref="Int64Pipe"/> creates a pipe which converts a string to a long.
        /// </summary>
        /// <param name="fmtStr">the format string</param>
        /// <param name="provider">the format provider for the converter</param>
        /// <returns>the pipe</returns>
        IUnDiPipe<string, long> Int64Pipe(string fmtStr = null, IFormatProvider provider = null);
        
        /// <summary>
        /// The <see cref="UInt64Pipe"/> creates a pipe which converts a string to an ulong.
        /// </summary>
        /// <param name="fmtStr">the format string</param>
        /// <param name="provider">the format provider for the converter</param>
        /// <returns>the pipe</returns>
        IUnDiPipe<string, ulong> UInt64Pipe(string fmtStr = null, IFormatProvider provider = null);
        
        /// <summary>
        /// The <see cref="EnumPipe{T}"/> method creates a pipe which converts a string to an enum.
        /// </summary>
        /// <returns>the pipe</returns>
        IUnDiPipe<string, T> EnumPipe<T>() where T : Enum;
        
        /// <summary>
        /// The <see cref="StringPipe"/> method creates the seed point for simple string parsing.
        /// </summary>
        /// <returns>the string pipe</returns>
        IUnDiPipe<string, string> StringPipe();
    }
}