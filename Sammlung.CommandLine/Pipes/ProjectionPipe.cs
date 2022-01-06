using System.Collections.Generic;
using System.Linq;
using Sammlung.Pipes;

namespace Sammlung.CommandLine.Pipes
{
    public class ProjectionPipe<T> : IUnDiPipe<IEnumerable<T>, T>
    {
        private readonly T _defaultValue;

        public ProjectionPipe(T defaultValue = default)
        {
            _defaultValue = defaultValue;
        }
        
        public T Process(IEnumerable<T> input) => input.FirstOrDefault() ?? _defaultValue;

    }
}