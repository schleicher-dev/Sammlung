using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Sammlung.Queues;
using Sammlung.Queues.Concurrent;

namespace _Benchmark.Sammlung
{
    [ExcludeFromCodeCoverage]
    public class ConcurrentDequeBenchmark
    {
        [Params(500, 1_000)]
        // ReSharper disable once UnassignedField.Global
        public int N;

        [Benchmark]
        public void PushAll_ArrayDeque()
        {
            var deque = new ArrayDeque<int>(N);
            for (var i = 0; i < N; ++i) deque.PushLeft(i);
        }

        [Benchmark]
        public void PushAll_BlockingDeque()
        {
            var deque = BlockingDequeExtensions.Wrap(new ArrayDeque<int>(N));
            Parallel.For(0, N, i => deque.PushLeft(i));
        }

        [Benchmark]
        public void PushAll_LockFreeDeque()
        {
            var deque = new LockFreeLinkedDeque<int>();
            Parallel.For(0, N, i => deque.PushLeft(i));
        }
    }
}