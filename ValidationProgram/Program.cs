using ValidationProgram.Validators;
using ValidationProgram.Validators.Opti;

namespace ValidationProgram;
internal class Program {
    private static void Main(string[] args) {
        string[] arguments = new string[] { "--count", "10", "--name", "cookies", "--helP" };
        var result = OptiValidateArguments.Validate(arguments);
        Console.WriteLine(result);
    }
}