namespace ValidationProgram.Validators.Opti;
public class OptiValidateArguments {
    public static int Validate(string[] args) {
        bool useHelp = false;
        bool useCount = false;
        bool useName = false;
        string[] parameters = new string[args.Length]; // Store parameters (args starting with "--")
        int paraIndex = 0;
        for(int i = 0; i < args.Length; i++) {
            if(args[i].StartsWith("--")) { // If argument is parameter
                args[i] = args[i].ToLower(); // Replace parameter value with its lowered result
                parameters[paraIndex] = args[i];
                paraIndex++;
                if(args[i].Equals("--help")) { // Find --help
                    useHelp = true;
                    continue;
                }
                if(args[i].Equals("--count")) { // Find --count
                    useCount = true;
                    continue;
                }
                if(args[i].Equals("--name")) { // Find --name
                    useName = true;
                    continue;
                }
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
            if(args[i].Equals("--name") || args[i].Equals("--count")) {
                if(args[i + 1].StartsWith("--")) // If argument after "--count" or "--name" is a param (NOT a value)
                    return true;
            } else if(args[i].Equals("--help")) {
                if(i + 1 < args.Length && !args[i + 1].StartsWith("--"))
                    // If "--help" is NOT the last argument AND if argument after "--help" is NOT a param
                    return true;
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
            if(!parameters[i].Equals("--count") && !parameters[i].Equals("--name") && !parameters[i].Equals("--help"))
                return true; // Invalid param
        }
        return false; // If we got this far then params are valid
    }
    private static bool ContainsInvalidValue(string[] args) {
        int countValueIndex;
        int nameValueIndex;
        for(int i = 0; i < args.Length; i++) {
            if(args[i].StartsWith("--")) { // If parameter
                if(args[i].Equals("--count")) { // If argument is "--count" parameter
                    countValueIndex = i + 1; // Index of passed count value
                    if(args[countValueIndex].StartsWith("--")) // If argument passed after "--count" also starts with "--" then it is not a value
                        return true;
                    if(int.TryParse(args[countValueIndex], out int result)) {
                        if(result < 10 || result > 100)
                            return true; // If count value is less than 10 or more than 100
                    } else
                        return true; // If count value could not be parsed to int
                }
                if(args[i].Equals("--name")) { // If argument is "--name" parameter
                    nameValueIndex = i + 1; // Index of passed name value
                    if(args[nameValueIndex].StartsWith("--")) // If argument passed after "--name" also starts with "--" then it is not a value
                        return true;
                    if(args[nameValueIndex].Length < 3 || args[nameValueIndex].Length > 10)
                        return true; // If name value is less than 3 chars in length or more than 10 chars in length
                }
            }
        }
        return false;
    }
}