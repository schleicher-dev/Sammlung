using System;
using Sammlung.Pipes;
using Sammlung.Pipes.SpecialPipes;
using Sammlung.Pipes.Werkzeug.Converters;

namespace Sammlung.CommandLine.Pipes
{
    public class PipeFactory : IPipeFactory
    {
        private static IUnDiPipe<string, T1> CreatePipeFrom<T1>(IBiDiPipe<string, T1> pipe) =>
            pipe.ForwardPipe();
        
        public IUnDiPipe<string, string> StringPipe() =>
            CreatePipeFrom(new IdentityPipe<string>()).WithDoubleQuotesRemoval();
        
        public IUnDiPipe<string, bool> BoolPipe(IFormatProvider provider = null) =>
            CreatePipeFrom(new StringToBooleanConverterPipe(provider));

        public IUnDiPipe<string, float> SinglePipe(string formatString = null, IFormatProvider provider = null) =>
            CreatePipeFrom(new StringToSingleConverterPipe(formatString, provider));

        public IUnDiPipe<string, double> DoublePipe(string fmtStr = null, IFormatProvider provider = null) =>
            CreatePipeFrom(new StringToDoubleConverterPipe(fmtStr, provider));

        public IUnDiPipe<string, decimal> DecimalPipe(string fmtStr = null, IFormatProvider provider = null) =>
            CreatePipeFrom(new StringToDecimalConverterPipe(fmtStr, provider));

        public IUnDiPipe<string, char> CharPipe(IFormatProvider fmtStr = null) =>
            CreatePipeFrom(new StringToCharConverterPipe(fmtStr));

        public IUnDiPipe<string, sbyte> SBytePipe(string fmtStr = null, IFormatProvider provider = null) =>
            CreatePipeFrom(new StringToSByteConverterPipe(fmtStr, provider));

        public IUnDiPipe<string, byte> BytePipe(string fmtStr = null, IFormatProvider provider = null) =>
            CreatePipeFrom(new StringToByteConverterPipe(fmtStr, provider));

        public IUnDiPipe<string, short> Int16Pipe(string fmtStr = null, IFormatProvider provider = null) =>
            CreatePipeFrom(new StringToInt16ConverterPipe(fmtStr, provider));

        public IUnDiPipe<string, ushort> UInt16Pipe(string fmtStr = null, IFormatProvider provider = null) =>
            CreatePipeFrom(new StringToUInt16ConverterPipe(fmtStr, provider));

        public IUnDiPipe<string, int> Int32Pipe(string fmtStr = null, IFormatProvider provider = null) =>
            CreatePipeFrom(new StringToInt32ConverterPipe(fmtStr, provider));

        public IUnDiPipe<string, uint> UInt32Pipe(string fmtStr = null, IFormatProvider provider = null) =>
            CreatePipeFrom(new StringToUInt32ConverterPipe(fmtStr, provider));

        public IUnDiPipe<string, long> Int64Pipe(string fmtStr = null, IFormatProvider provider = null) =>
            CreatePipeFrom(new StringToInt64ConverterPipe(fmtStr, provider));

        public IUnDiPipe<string, ulong> UInt64Pipe(string fmtStr = null, IFormatProvider provider = null) =>
            CreatePipeFrom(new StringToUInt64ConverterPipe(fmtStr, provider));

    }
}