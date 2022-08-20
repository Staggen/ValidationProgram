namespace ConsoleTestApp.Validators;
public class ValidateArguments {
    public static int Validate(string[] args) {
        List<string> loweredArgs = new();
        foreach(string arg in args)
            loweredArgs.Add(arg.ToLower()); // Convert all arguments to lower case letters only
        // Check which of the 3 valid parameters have been requested
        bool useHelp = loweredArgs.Any(arg => arg.Equals("--help"));
        bool useCount = loweredArgs.Any(arg => arg.Equals("--count"));
        bool useName = loweredArgs.Any(arg => arg.Equals("--name"));
        // Examine if bad argument count, order or bad parameter
        if(ContainsInvalidArgumentCount(loweredArgs, useCount, useName, useHelp))
            return -1;
        if(ContainsInvalidArgumentOrder(loweredArgs))
            return -1;
        if(ContainsInvalidParameter(loweredArgs.Where(arg => arg.StartsWith("--")).ToList()))
            return -1;
        // Examine if bad value passed with "--count"
        if(useCount) { // If parameter "--count" used
            int index = loweredArgs.FindIndex(x => x.Equals("--count"));
            if(IsParam(loweredArgs[index + 1])) // If argument passed after the "--count" param starts with -- and therefore is a param
                return -1; // If bad value then return -1
            if(int.TryParse(loweredArgs[index + 1], out int result)) { // Otherwise try to convert string to integer
                if(ContainsInvalidNumber(result)) // If integer is less than 10 OR more than 100
                    return -1; // If bad value then return -1
            } else // If unable to parse value to int then bad value
                return -1; // If bad value then return -1
        }
        // Examine if bad value passed with "--name"
        if(useName) { // If parameter "--name" used
            int index = loweredArgs.FindIndex(x => x.Equals("--name"));
            if(IsParam(loweredArgs[index + 1])) // If argument passed after the --name param is NOT a value but another param
                return -1; // If bad value then return -1
            if(ContainsInvalidName(loweredArgs[index + 1])) // If string shorter than 3 chars OR longer than 10
                return -1; // If bad value then return -1
        }
        // If we got this far then parameters and values are valid. If help requested then return 1
        if(useHelp)
            return 1;
        // If help NOT requested but everything is good otherwise then return 0
        return 0;
    }
    private static bool IsParam(string possibleParameter) {
        return possibleParameter.StartsWith("--");
    }
    private static bool ContainsInvalidArgumentCount(List<string> arguments, bool useCount, bool useName, bool useHelp) {
        // If argument count invalid then it is guaranteed to contain bad arguments
        if(!useCount && !useName && !useHelp) // If sending NO arguments at all
            return true;
        if(!useCount && !useName && useHelp && arguments.Count != 1) // If JUST using "--help" and argument count NOT equal to 1
            return true;
        if(useCount && !useName && !useHelp && arguments.Count != 2) // If JUST using "--count" and argument count NOT equal to 2
            return true;
        if(!useCount && useName && !useHelp && arguments.Count != 2) // If JUST using "--name" and argument count NOT equal to 2
            return true;
        if(useCount && !useName && useHelp && arguments.Count != 3) // "--count" + "--help" and argument count NOT equal to 3
            return true;
        if(!useCount && useName && useHelp && arguments.Count != 3) // "--name" + "--help" and argument count NOT equal to 3
            return true;
        if(useCount && useName && !useHelp && arguments.Count != 4) // "--count" + "--name" and argument count NOT equal to 4
            return true;
        if(useCount && useName && useHelp && arguments.Count != 5) // "--count" + "--name" + "--help" and argument count NOT equal to 5
            return true;
        // If nothing has gone wrong so far then argument count is valid
        return false;
    }
    private static bool ContainsInvalidArgumentOrder(List<string> loweredArguments) {
        // If argument order invalid then guaranteed to contain bad arguments
        for(int i = 0; i < loweredArguments.Count; i++) {
            if(loweredArguments[i].Equals("--name") || loweredArguments[i].Equals("--count")) {
                if(loweredArguments[i + 1].StartsWith("--")) // If argument after "--count" or "--name" is a param (NOT a value)
                    return true;
            } else if(loweredArguments[i].Equals("--help")) {
                if(i + 1 < loweredArguments.Count && !loweredArguments[i + 1].StartsWith("--"))
                    // If "--help" is NOT the last argument AND if argument after "--help" is NOT a param
                    return true;
            }
        }
        // If nothing has gone wrong so far then argument order is valid
        return false;
    }
    private static bool ContainsInvalidParameter(List<string> parameters) { // If the parameters provided matches any of the acceptable params
        if(parameters.Count > 3) // If more than 3 params it is guaranteed to contain bad params
            return true;
        foreach(string parameter in parameters) {
            // If parameter is NOT "--count", NOT "--name" and NOT "--help"
            if(!parameter.Equals("--count") && !parameter.Equals("--name") && !parameter.Equals("--help"))
                return true; // Invalid param
        }
        return false; // If we got this far params are valid
    }
    private static bool ContainsInvalidNumber(int number) { // If number less than 10 OR more than 100
        return number < 10 || number > 100;
    }
    private static bool ContainsInvalidName(string name) { // If name.Length less than 3 chars OR more than 10 chars
        return name.Length < 3 || name.Length > 10;
    }
}