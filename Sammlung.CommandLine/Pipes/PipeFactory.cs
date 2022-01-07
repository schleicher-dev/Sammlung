using System;
using System.Linq;
using Sammlung.Pipes;
using Sammlung.Pipes.SpecialPipes;
using Sammlung.Pipes.Werkzeug.Converters;
using Sammlung.Werkzeug;

namespace Sammlung.CommandLine.Pipes
{
    /// <summary>
    /// The <see cref="PipeFactory"/> is a class which exposes the <see cref="IPipeFactory"/> en plus one is able
    /// to add common preprocessing steps to each of the pipes created.
    /// </summary>
    public class PipeFactory : IPipeFactory
    {

        private readonly IUnDiPipe<string, string> _commonPreprocessingStep;

        /// <summary>
        /// Creates a new <see cref="PipeFactory"/> using a bunch of pre-processing steps.
        /// </summary>
        /// <param name="commonPreprocessingSteps">the pre-processing steps</param>
        /// <remarks>
        /// A pre-processing step is a <see cref="IUnDiPipe{TSource,TTarget}"/> which is called directly after
        /// the token is passed to the initial <see cref="IUnDiPipe{TSource,TTarget}"/> right before the
        /// conversion stage of the pipe.
        /// </remarks>
        public PipeFactory(params IUnDiPipe<string, string>[] commonPreprocessingSteps)
        {
            commonPreprocessingSteps = commonPreprocessingSteps.RequireNotNull(nameof(commonPreprocessingSteps));
            _commonPreprocessingStep = CreateCommonPreprocessingPipe(commonPreprocessingSteps);
        }

        private static IUnDiPipe<string, string> DefaultPipe() => new RemoveDoubleQuotesPipe();
        
        private static IUnDiPipe<string, string> CreateCommonPreprocessingPipe(IUnDiPipe<string, string>[] pipes) => 
            !pipes.Any() ? DefaultPipe() : pipes.Aggregate(DefaultPipe(), (a, p) => a.Append(p));

        private IUnDiPipe<string, T1> CreatePipeFrom<T1>(IBiDiPipe<string, T1> pipe) =>
            _commonPreprocessingStep.Append(pipe.ForwardPipe());

        /// <inheritdoc />
        public IUnDiPipe<string, string> StringPipe() => CreatePipeFrom(new IdentityPipe<string>());
        
        /// <inheritdoc />
        public IUnDiPipe<string, bool> BoolPipe(IFormatProvider provider = null) =>
            CreatePipeFrom(new StringToBooleanConverterPipe(provider));

        /// <inheritdoc />
        public IUnDiPipe<string, float> SinglePipe(string fmtStr = null, IFormatProvider provider = null) =>
            CreatePipeFrom(new StringToSingleConverterPipe(fmtStr, provider));

        /// <inheritdoc />
        public IUnDiPipe<string, double> DoublePipe(string fmtStr = null, IFormatProvider provider = null) =>
            CreatePipeFrom(new StringToDoubleConverterPipe(fmtStr, provider));

        /// <inheritdoc />
        public IUnDiPipe<string, decimal> DecimalPipe(string fmtStr = null, IFormatProvider provider = null) =>
            CreatePipeFrom(new StringToDecimalConverterPipe(fmtStr, provider));

        /// <inheritdoc />
        public IUnDiPipe<string, char> CharPipe(IFormatProvider fmtStr = null) =>
            CreatePipeFrom(new StringToCharConverterPipe(fmtStr));

        /// <inheritdoc />
        public IUnDiPipe<string, sbyte> SBytePipe(string fmtStr = null, IFormatProvider provider = null) =>
            CreatePipeFrom(new StringToSByteConverterPipe(fmtStr, provider));

        /// <inheritdoc />
        public IUnDiPipe<string, byte> BytePipe(string fmtStr = null, IFormatProvider provider = null) =>
            CreatePipeFrom(new StringToByteConverterPipe(fmtStr, provider));

        /// <inheritdoc />
        public IUnDiPipe<string, short> Int16Pipe(string fmtStr = null, IFormatProvider provider = null) =>
            CreatePipeFrom(new StringToInt16ConverterPipe(fmtStr, provider));

        /// <inheritdoc />
        public IUnDiPipe<string, ushort> UInt16Pipe(string fmtStr = null, IFormatProvider provider = null) =>
            CreatePipeFrom(new StringToUInt16ConverterPipe(fmtStr, provider));

        /// <inheritdoc />
        public IUnDiPipe<string, int> Int32Pipe(string fmtStr = null, IFormatProvider provider = null) =>
            CreatePipeFrom(new StringToInt32ConverterPipe(fmtStr, provider));

        /// <inheritdoc />
        public IUnDiPipe<string, uint> UInt32Pipe(string fmtStr = null, IFormatProvider provider = null) =>
            CreatePipeFrom(new StringToUInt32ConverterPipe(fmtStr, provider));

        /// <inheritdoc />
        public IUnDiPipe<string, long> Int64Pipe(string fmtStr = null, IFormatProvider provider = null) =>
            CreatePipeFrom(new StringToInt64ConverterPipe(fmtStr, provider));

        /// <inheritdoc />
        public IUnDiPipe<string, T> EnumPipe<T>() where T : Enum => 
            CreatePipeFrom(new StringToEnumConverterPipe<T>());

        /// <inheritdoc />
        public IUnDiPipe<string, ulong> UInt64Pipe(string fmtStr = null, IFormatProvider provider = null) =>
            CreatePipeFrom(new StringToUInt64ConverterPipe(fmtStr, provider));

    }
}