namespace ValidationProgram.Validators.Opti;
public class OptiValidateArguments {
    public static string[] GetParametersUsingStartsWith(string[] args) {
        string[] parameters = new string[args.Length];
        int paramCountTracker = 0;
        for(int i = 0; i < args.Length; i++) {
            if(!args[i].StartsWith("--"))
                continue;
            args[i] = args[i].ToLower();
            parameters[paramCountTracker] = args[i];
            paramCountTracker++;
        }
        return parameters;
    }
    public static string[] GetParametersUsingSubstring(string[] args) {
        string[] parameters = new string[args.Length];
        int paramCountTracker = 0;
        for(int i = 0; i < args.Length; i++) {
            if(args[i].Length < 2)
                continue;
            if(!args[i].Substring(0, 2).Equals("--"))
                continue;
            args[i] = args[i].ToLower();
            parameters[paramCountTracker] = args[i];
            paramCountTracker++;
        }
        return parameters;
    }
    public static string[] GetParametersUsingSpan(string[] args) {
        string[] parameters = new string[args.Length];
        int paramCountTracker = 0;
        for(int i = 0; i < args.Length; i++) {
            var argAsSpan = args[i].AsSpan();
            if(argAsSpan.Length < 2)
                continue;
            var slice = argAsSpan.Slice(0, 2);
            if(slice[0] == '-' && slice[1] == '-')
                continue;
            args[i] = args[i].ToLower();
            parameters[paramCountTracker] = args[i];
            paramCountTracker++;
        }
        return parameters;
    }

    public static int Validate(string[] args) {
        if(args.Length == 0) // If no arguments passed
            return -1;
        if(args[0].Length == 0) // If empty string argument passed
            return -1;
        bool useHelp = false;
        bool useCount = false;
        bool useName = false;
        string[] parameters = new string[args.Length]; // Store parameters (args starting with "--")
        int paraIndex = 0;
        for(int i = 0; i < args.Length; i++) {
            var valueAsSpan = args[i].AsSpan();
            if(valueAsSpan.Length < 6) { // Shortest parameter is "--help" and "--name", both at 6 characters in length
                continue;
            }
            var slice = valueAsSpan[..2];
            if(slice[0] != '-' && slice[1] != '-') // If argument does NOT start with "--" and therefore is a value
                continue;
            args[i] = args[i].ToLower(); // Replace parameter value with its lowered result
            parameters[paraIndex] = args[i];
            paraIndex++;
            if(args[i] == "--help") { // Find --help
                useHelp = true;
                continue;
            }
            if(args[i] == "--count") { // Find --count
                useCount = true;
                continue;
            }
            if(args[i] == "--name") { // Find --name
                useName = true;
                continue;
            }
        }
        // Examine if bad argument count, bad argument order, bad parameter or bad value
        if(ContainsInvalidArgumentCount(args.Length, useCount, useName, useHelp))
            return -1;
        if(ContainsInvalidArgumentOrder(args))
            return -1;
        Array.Resize(ref parameters, paraIndex); // If we got this far remove the null slots in the parameters array
        if(ContainsInvalidParameter(parameters))
            return -1;
        if(ContainsInvalidValue(args))
            return -1;
        // If we got this far then parameters and values are valid. If help requested then return 1
        if(useHelp)
            return 1;
        // If help NOT requested but everything is good otherwise then return 0
        return 0;
    }
    private static bool ContainsInvalidArgumentCount(int argsCount, bool useCount, bool useName, bool useHelp) {
        // If argument count invalid then it is guaranteed to contain bad arguments
        if(!useCount && !useName && !useHelp) // If sending NO arguments at all
            return true;
        if(!useCount && !useName && useHelp && argsCount != 1) // If JUST using "--help" and argument count NOT equal to 1
            return true;
        if(useCount && !useName && !useHelp && argsCount != 2) // If JUST using "--count" and argument count NOT equal to 2
            return true;
        if(!useCount && useName && !useHelp && argsCount != 2) // If JUST using "--name" and argument count NOT equal to 2
            return true;
        if(useCount && !useName && useHelp && argsCount != 3) // "--count" + "--help" and argument count NOT equal to 3
            return true;
        if(!useCount && useName && useHelp && argsCount != 3) // "--name" + "--help" and argument count NOT equal to 3
            return true;
        if(useCount && useName && !useHelp && argsCount != 4) // "--count" + "--name" and argument count NOT equal to 4
            return true;
        if(useCount && useName && useHelp && argsCount != 5) // "--count" + "--name" + "--help" and argument count NOT equal to 5
            return true;
        // If nothing has gone wrong so far then argument count is valid
        return false;
    }
    private static bool ContainsInvalidArgumentOrder(string[] args) {
        // If argument order invalid then guaranteed to contain bad arguments
        for(int i = 0; i < args.Length; i++) {
            if(args[i].Length < 6) // If arg length is less than 6 then it can't be a param
                continue;
            if(args[i] == "--count") {
                if((i + 1) >= args.Length) // "--count" can not be last argument as it requires a value
                    return true;
                if(args[i + 1].Length < 2 || args[i + 1].Length > 3) // Should be number between 10 and 100 so must be 2 or 3 characters long
                    return true;
                var nextArgAsSpan = args[i + 1].AsSpan();
                var slice = nextArgAsSpan.Slice(0, 2);
                if(slice[0] == '-' && slice[1] == '-') // If arg is a parameter (and not a value)
                    return true;
            } else if(args[i] == "--name") {
                if((i + 1) >= args.Length) // "--name" can not be last argument as it requires a value
                    return true;
                if(args[i + 1].Length < 3 || args[i + 1].Length > 10) // Should be string between 3 and 10 characters long
                    return true;
                var nextArgAsSpan = args[i + 1].AsSpan();
                var slice = nextArgAsSpan.Slice(0, 2);
                if(slice[0] == '-' && slice[1] == '-') // If arg is a parameter (and not a value)
                    return true;
            } else if(args[i] == "--help") {
                if((i + 1) < args.Length) { // If arg after this "--help" arg exists
                    var nextArgAsSpan = args[i + 1].AsSpan();
                    // If argument after "--help" is NOT a param
                    if(nextArgAsSpan.Length < 6) // Arg of less than 6 characters cannot be valid parameter
                        return true;
                    var slice = nextArgAsSpan.Slice(0, 2);
                    if(slice[0] != '-' && slice[1] != '-') // Arg after "--help" should always be parameter and start with "--"
                        return true;
                }
            }
        }
        // If nothing has gone wrong so far then argument order is valid
        return false;
    }
    private static bool ContainsInvalidParameter(string[] parameters) { // If the parameters provided matches any of the acceptable params
        if(parameters.Length > 3) // If more than 3 params it is guaranteed to contain bad params
            return true;
        for(int i = 0; i < parameters.Length; i++) {
            // If parameter is NOT "--count", NOT "--name" and NOT "--help"
            if(parameters[i].Equals("--count"))
                continue;
            if(parameters[i].Equals("--name"))
                continue;
            if(parameters[i].Equals("--help"))
                continue;
            return true;
        }
        return false; // If we got this far then params are valid
    }
    private static bool ContainsInvalidValue(string[] args) {
        for(int i = 0; i < args.Length; i++) {
            if(args[i].Length < 6) // If value, cycle through
                continue;
            var argAsSpan = args[i].AsSpan();
            if(argAsSpan.Length >= 6) { // If right length to be param
                var slice = argAsSpan.Slice(0, 2);
                if(slice[0] != '-' && slice[1] != '-') // If not parameter
                    continue;
            }
            if(args[i] == "--count") { // If argument is "--count" parameter
                if((i + 1) >= args.Length) // If no argument after "--count"
                    return true; // No value is invalid value
                var nextArgAsSpan = args[i + 1].AsSpan();
                if(nextArgAsSpan.Length < 2) // Value of less than 2 characters can not be valid "--count" value
                    return true;
                var slice = nextArgAsSpan.Slice(0, 2);
                if(slice[0] == '-' && slice[1] == '-') // If argument passed after "--count" also starts with "--" then it is not a value
                    return true;
                if(!int.TryParse(args[i + 1], out int result)) // If unable to parse value to int
                    return true;
                if(result < 10 || result > 100) // If count value is less than 10 or more than 100
                    return true;
            }
            if(args[i].Equals("--name")) { // If argument is "--name" parameter
                if((i + 1) >= args.Length) // If no argument after "--name"
                    return true; // No value is invalid value
                var nextArgAsSpan = args[i + 1].AsSpan();
                if(nextArgAsSpan.Length < 3) // Value of less than 3 characters can not be valid "--name" value
                    return true;
                var slice = nextArgAsSpan.Slice(0, 2);
                if(slice[0] == '-' && slice[1] == '-') // If argument passed after "--name" also starts with "--" then it is not a value
                    return true;
                if(nextArgAsSpan.Length > 10) // Value of more than 10 characters can not be valid "--name" value
                    return true;
            }
        }
        return false;
    }
}