using BenchmarkDotNet.Running;

namespace ValidationProgram;
internal class Program {
    public static void Main(string[] args) {
        BenchmarkRunner.Run<ValidationBenchmarks>();
    }
}