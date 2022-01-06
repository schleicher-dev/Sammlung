using System.Collections.Generic;
using System.Linq;
using Sammlung.Pipes;
using Sammlung.Werkzeug;

namespace Sammlung.CommandLine.Pipes
{
    public class EnumerablePipe<T1, T2> : IUnDiPipe<IEnumerable<T1>, IEnumerable<T2>>
    {
        private readonly IUnDiPipe<T1, T2> _converterPipe;

        public EnumerablePipe(IUnDiPipe<T1, T2> converterPipe)
        {
            _converterPipe = converterPipe.RequireNotNull(nameof(converterPipe));
        }
        
        public IEnumerable<T2> Process(IEnumerable<T1> input) => 
            input.Select(_converterPipe.Process);
    }
}