using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using ValidationProgram.Validators;
using ValidationProgram.Validators.Opti;

namespace ValidationProgram;
[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class ValidationBenchmarks {
    private readonly string[] _args = new string[] { "--count", "10", "--name", "cookies", "--helP" };

    [Benchmark]
    public void GetValidationResultFromInput() {
        _ = ValidateArguments.Validate(_args);
    }
    [Benchmark]
    public void OptimizedGetValidationResultFromInput() {
        _ = OptiValidateArguments.Validate(_args);
    }
    //[Benchmark]
    //public void GetParametersUsingStartsWithBenchmark() {
    //    _ = OptiValidateArguments.GetParametersUsingStartsWith(_args);
    //}
    //[Benchmark]
    //public void GetParametersUsingSubstringBenchmark() {
    //    _ = OptiValidateArguments.GetParametersUsingSubstring(_args);
    //}
    //[Benchmark]
    //public void GetParametersUsingSpanBenchmark() {
    //    _ = OptiValidateArguments.GetParametersUsingSpan(_args);
    //}
}