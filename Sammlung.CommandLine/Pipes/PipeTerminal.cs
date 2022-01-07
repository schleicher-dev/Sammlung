using System;
using Sammlung.CommandLine.Exceptions;
using Sammlung.CommandLine.Resources;
using Sammlung.Pipes;
using Sammlung.Pipes.SpecialPipes;
using Sammlung.Werkzeug;

namespace Sammlung.CommandLine.Pipes
{
    /// <summary>
    /// The <see cref="PipeTerminal{TData,T1,T2}"/> invokes an underlying pipe and adds its value to the data.
    /// </summary>
    /// <typeparam name="TData">the data type</typeparam>
    /// <typeparam name="T1">the pipe input type</typeparam>
    /// <typeparam name="T2">the pipe output type</typeparam>
    public class PipeTerminal<TData, T1, T2> : IPipeTerminal<TData, T1>
    {
        private TData _data;
        private readonly IUnDiPipe<T1,T2> _conversionPipe;
        private readonly IUnDiPipe<T2, T2> _validationPipe;

        private readonly Func<TData, T2> _getValue;
        private readonly Action<TData, T2> _setValue;

        /// <summary>
        /// Creates a new <see cref="PipeTerminal{TData,T1,T2}"/> using a pipe and a delegate which pushes the
        /// values to the data.
        /// </summary>
        /// <param name="conversionPipe">the conversion pipe</param>
        /// <param name="validationPipe">the validation pipe</param>
        /// <param name="getValue">the delegate which gets the value</param>
        /// <param name="setValue">the delegate which sets the value</param>
        public PipeTerminal(IUnDiPipe<T1, T2> conversionPipe, IUnDiPipe<T2, T2> validationPipe, Func<TData, T2> getValue, Action<TData, T2> setValue)
        {
            _conversionPipe = conversionPipe.RequireNotNull(nameof(conversionPipe));
            _validationPipe = validationPipe ?? new IdentityPipe<T2>().ForwardPipe();
            _getValue = getValue.RequireNotNull(nameof(getValue));
            _setValue = setValue.RequireNotNull(nameof(setValue));
        }
        
        /// <inheritdoc />
        public void Bind(TData data)
        {
            _data = data;
        }

        private void RequireBinding()
        {
            if (_data == null)
                throw new GenericException(Lang.ObjectIsUnbound);
        }

        /// <inheritdoc />
        public void ExecuteConversionStage(T1 input)
        {
            RequireBinding();
            var value = _conversionPipe.Process(input);
            _setValue.Invoke(_data, value);
        }

        /// <inheritdoc />
        public void ExecuteValidationStage()
        {
            RequireBinding();
            var value = _getValue.Invoke(_data);
            var validated = _validationPipe.Process(value);
            _setValue.Invoke(_data, value);
        }

        /// <inheritdoc />
        public void ExecuteAll(T1 input)
        {
            ExecuteConversionStage(input);
            ExecuteValidationStage();
        }
    }
}