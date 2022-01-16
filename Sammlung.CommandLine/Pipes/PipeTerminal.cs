using System;
using Sammlung.CommandLine.Exceptions;
using Sammlung.CommandLine.Reflection;
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
        private readonly Property<TData, T2> _property;
        private readonly IUnDiPipe<T1,T2> _conversionPipe;
        private readonly IUnDiPipe<T2, T2> _validationPipe;
        
        /// <summary>
        /// Creates a new <see cref="PipeTerminal{TData,T1,T2}"/> using a pipe and a delegate which pushes the
        /// values to the data.
        /// </summary>
        /// <param name="conversionPipe">the conversion pipe</param>
        /// <param name="validationPipe">the validation pipe</param>
        /// <param name="property">the property which can be read and written</param>
        public PipeTerminal(IUnDiPipe<T1, T2> conversionPipe, IUnDiPipe<T2, T2> validationPipe, Property<TData, T2> property)
        {
            _conversionPipe = conversionPipe.RequireNotNull(nameof(conversionPipe));
            _validationPipe = validationPipe ?? new IdentityPipe<T2>().ForwardPipe();
            _property = property.RequireNotNull(nameof(property));
        }
        
        /// <inheritdoc />
        public void Bind(TData data) => _property.Bind(data);

        /// <inheritdoc />
        public void ExecuteConversionStage(T1 input) => _property.Value = _conversionPipe.Process(input);

        /// <inheritdoc />
        public void ExecuteValidationStage()
        {
            var value = _property.Value;
            _property.Value = _validationPipe.Process(value);
        }

        /// <inheritdoc />
        public void ExecuteAll(T1 input)
        {
            ExecuteConversionStage(input);
            ExecuteValidationStage();
        }
    }
}