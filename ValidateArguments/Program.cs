using ConsoleTestApp.Validators;

namespace ConsoleTestApp;
internal class Program {
    private static void Main(string[] args) {
        string[] arguments = new string[] { "--count", "10", "--name", "cookies", "--helP" };
        var result = ValidateArguments.Validate(arguments);
        Console.WriteLine(result);
    }
}