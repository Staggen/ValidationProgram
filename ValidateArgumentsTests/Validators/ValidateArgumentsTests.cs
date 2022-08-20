using ConsoleTestApp.Validators;

namespace ConsoleTestAppTests.Validators;
public class ValidateArgumentsTests {
    [Theory]
    [InlineData("--count", "14")]
    [InlineData("--name", "Steve")]
    [InlineData("--count", "14", "--name", "Steve")]
    [InlineData("--name", "Steve", "--count", "14")]
    [InlineData("--cOUnt", "14")]
    [InlineData("--nAme", "Steve")]
    [InlineData("--CouNt", "14", "--naMe", "Steve")]
    [InlineData("--namE", "Steve", "--coUnt", "14")]
    public void ValidArgumentsWithoutHelp_Should_ReturnZero(params string[] args) {
        // Arrange
        // Act
        var result = ValidateArguments.Validate(args);
        // Assert
        Assert.Equal(0, result);
    }
    [Theory]
    [InlineData("--help")]
    [InlineData("--count", "14", "--help")]
    [InlineData("--help", "--count", "14")]
    [InlineData("--name", "Steve", "--help")]
    [InlineData("--help", "--name", "Steve")]
    [InlineData("--count", "14", "--name", "Steve", "--help")]
    [InlineData("--help", "--count", "14", "--name", "Steve")]
    [InlineData("--count", "14", "--help", "--name", "Steve")]
    [InlineData("--hELp")]
    [InlineData("--CoUNt", "14", "--HelP")]
    [InlineData("--nAmE", "Steve", "--hElP")]
    [InlineData("--couNT", "14", "--NAme", "Steve", "--HElp")]
    public void ValidArgumentsWithHelp_Should_ReturnOne(params string[] args) {
        // Arrange
        // Act
        var result = ValidateArguments.Validate(args);
        // Assert
        Assert.Equal(1, result);
    }
    [Theory]
    [InlineData("")]
    [InlineData("--name")]
    [InlineData("--count")]
    [InlineData("--name", "--help")]
    [InlineData("--count", "--help")]
    [InlineData("--count", "18", "--name")]
    [InlineData("--count", "--name", "Jimmy")]
    [InlineData("--count", "18", "--name", "--help")]
    [InlineData("--count", "18", "--count", "--22")]
    [InlineData("--count", "--name", "Jimmy", "--help")]
    [InlineData("--count", "87", "--name", "Jimmy", "Derp")]
    [InlineData("--count", "87", "--name", "Jimmy", "14")]
    [InlineData("--count", "87", "--name", "Jimmy", "--help", "Steve")]
    [InlineData("--count", "82", "steve")]
    [InlineData("--count", "82", "steve", "--help")]
    [InlineData("--name", "Bobby", "steve")]
    [InlineData("--name", "Bobby", "steve", "--help")]
    public void InvalidArgumentCount_Should_ReturnMinusOne(params string[] args) {
        // Arrange
        // Act
        var result = ValidateArguments.Validate(args);
        // Assert
        Assert.Equal(-1, result);
    }
    [Theory]
    [InlineData("")]
    [InlineData("--name", "--help", "Whoopsie")]
    [InlineData("--help", "Steve", "--count")]
    [InlineData("--count", "--name", "18")]
    [InlineData("--count", "Jimmy", "--name")]
    [InlineData("--count", "87", "Jimmy", "--name", "--help")]
    [InlineData("--count", "--help", "87", "--name", "Jimmy")]
    public void InvalidArgumentOrder_Should_ReturnMinusOne(params string[] args) {
        // Arrange
        // Act
        var result = ValidateArguments.Validate(args);
        // Assert
        Assert.Equal(-1, result);
    }
    [Theory]
    [InlineData("--coount", "14")]
    [InlineData("--naame", "Steve")]
    [InlineData("--count", "14", "--nam1e", "Steve")]
    [InlineData("--nname", "Steve", "--count", "14")]
    [InlineData("--name", "Steve", "--count", "14", "--something", "random")]
    [InlineData("-n-Ame", "Steve")]
    [InlineData("--helpp")]
    [InlineData("--CouNt", "14", "--heelp", "--naMe", "Steve")]
    public void InvalidParameters_Should_ReturnMinusOne(params string[] args) {
        // Arrange
        // Act
        var result = ValidateArguments.Validate(args);
        // Assert
        Assert.Equal(-1, result);
    }
    [Theory]
    [InlineData("--count", "144")]
    [InlineData("--count", "1")]
    [InlineData("--count", "Steve")]
    [InlineData("--name", "Stephen 1234567890")]
    [InlineData("--name", "Ey")]
    [InlineData("--help", "12")]
    public void InvalidValues_Should_ReturnMinusOne(params string[] args) {
        // Arrange
        // Act
        var result = ValidateArguments.Validate(args);
        // Assert
        Assert.Equal(-1, result);
    }
}