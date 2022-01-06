using System;
using System.Collections.Generic;
using Sammlung.Pipes;

namespace Sammlung.CommandLine.Pipes
{
    public interface IPipeFactory
    {
        IUnDiPipe<string, bool> BoolPipe(IFormatProvider provider = null);
        IUnDiPipe<string, float> SinglePipe(string formatString = null, IFormatProvider provider = null);
        IUnDiPipe<string, double> DoublePipe(string fmtStr = null, IFormatProvider provider = null);
        IUnDiPipe<string, decimal> DecimalPipe(string fmtStr = null, IFormatProvider provider = null);
        IUnDiPipe<string, char> CharPipe(IFormatProvider fmtStr = null);
        IUnDiPipe<string, sbyte> SBytePipe(string fmtStr = null, IFormatProvider provider = null);
        IUnDiPipe<string, byte> BytePipe(string fmtStr = null, IFormatProvider provider = null);
        IUnDiPipe<string, short> Int16Pipe(string fmtStr = null, IFormatProvider provider = null);
        IUnDiPipe<string, ushort> UInt16Pipe(string fmtStr = null, IFormatProvider provider = null);
        IUnDiPipe<string, int> Int32Pipe(string fmtStr = null, IFormatProvider provider = null);
        IUnDiPipe<string, uint> UInt32Pipe(string fmtStr = null, IFormatProvider provider = null);
        IUnDiPipe<string, long> Int64Pipe(string fmtStr = null, IFormatProvider provider = null);
        IUnDiPipe<string, ulong> UInt64Pipe(string fmtStr = null, IFormatProvider provider = null);
        IUnDiPipe<string, string> StringPipe();
    }
}