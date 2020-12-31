using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Sammlung.Queues;
using Sammlung.Queues.Concurrent;

namespace _Benchmark.Sammlung
{
    public class ConcurrentDequeBenchmark
    {
        private int[] _data;
        
        [Params(500, 1_000)]
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
            var deque = BlockingDeque.Wrap(new ArrayDeque<int>(N));
            Parallel.For(0, N, i => deque.PushLeft(i));
        }

        [Benchmark]
        public void PushAll_LockFreeDeque()
        {
            var deque = new LockFreeDeque<int>();
            Parallel.For(0, N, i => deque.PushLeft(i));
        }
    }
}