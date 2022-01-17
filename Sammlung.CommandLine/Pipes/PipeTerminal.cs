using System;
using Sammlung.CommandLine.Exceptions;
using Sammlung.CommandLine.Resources;
using Sammlung.CommandLine.Utilities;
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
        private readonly BindableSetter<TData, T2> _bindableSetter;
        private readonly IUnDiPipe<T1,T2> _pipe;
        
        /// <summary>
        /// Creates a new <see cref="PipeTerminal{TData,T1,T2}"/> using a pipe and a delegate which pushes the
        /// values to the data.
        /// </summary>
        /// <param name="pipe">the conversion pipe</param>
        /// <param name="bindableSetter">the setter action</param>
        public PipeTerminal(IUnDiPipe<T1, T2> pipe, BindableSetter<TData, T2> bindableSetter)
        {
            _pipe = pipe.RequireNotNull(nameof(pipe));
            _bindableSetter = bindableSetter.RequireNotNull(nameof(bindableSetter));
        }
        
        /// <inheritdoc />
        public void Bind(TData data) => _bindableSetter.Bind(data);

        /// <inheritdoc />
        public void Execute(T1 input) =>  _bindableSetter.Value = _pipe.Process(input);
    }
}