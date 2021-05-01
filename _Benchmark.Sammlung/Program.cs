using System.Diagnostics.CodeAnalysis;
using BenchmarkDotNet.Running;

namespace _Benchmark.Sammlung
{
    [ExcludeFromCodeCoverage]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "PublicAPI")]
    public class Program
    {
        public static void Main()
        {
            var  _ = BenchmarkRunner.Run<ConcurrentDequeBenchmark>();
        }
    }
}