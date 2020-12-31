using BenchmarkDotNet.Running;

namespace _Benchmark.Sammlung
{
    public class Program
    {
        public static void Main()
        {
            var summary = BenchmarkRunner.Run<ConcurrentDequeBenchmark>();
        }
    }
}