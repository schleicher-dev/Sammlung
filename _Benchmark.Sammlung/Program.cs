using System.Diagnostics.CodeAnalysis;
using BenchmarkDotNet.Running;

namespace _Benchmark.Sammlung
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        public static void Main()
        {
            var  _ = BenchmarkRunner.Run<ConcurrentDequeBenchmark>();
        }
    }
}