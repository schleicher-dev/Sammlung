using Sammlung.Pipes;
using Sammlung.Pipes.Werkzeug.Exceptions;
using Sammlung.Werkzeug;

namespace Sammlung.CommandLine.Pipes
{
    public class PipeEndpoint<TData, T1, T2> : IPipeEndpoint<TData, T1>
    {
        private readonly IUnDiPipe<T1, T2> _pipe;
        private readonly PushValueDelegate<TData, T2> _pushValue;
        private TData _data;

        public PipeEndpoint(IUnDiPipe<T1, T2> pipe, PushValueDelegate<TData, T2> pushValue)
        {
            _pipe = pipe.RequireNotNull(nameof(pipe));
            _pushValue = pushValue.RequireNotNull(nameof(pushValue));
        }

        public void PushValue(T1 value)
        {
            if (_data == null)
                throw new PipelineValidationException();
            
            var result = _pipe.Process(value);
            _pushValue.Invoke(_data, result);
        }
        
        public void Bind(TData data)
        {
            _data = data;
        }

    }
}