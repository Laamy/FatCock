using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class FC_State
{
    private Stack<string> Stack = new Stack<string>();

    private Stack<List<object>> Imports = new Stack<List<object>>();

    // define queue for script execution
    public Queue<string> ExecutionQueue = new Queue<string>();

    public void Push(string state, string type)
    {
        Stack.Push(state);
        Stack.Push(type);
    }

    public KeyValuePair<string, string> Pop()
    {
        if (Stack.Count == 0)
            throw new InvalidOperationException("Stack is empty");

        return new KeyValuePair<string, string>(Stack.Pop(), Stack.Pop());
    }

    public void PushImport(List<object> imports) => Imports.Push(imports);
    public List<object> PopImport()
    {
        if (Imports.Count == 0)
            throw new InvalidOperationException("Imports stack is empty");

        return Imports.Pop();
    }

    public Type GetImport(string className)
    {
        foreach (List<object> import in Imports)
        {
            if (import[0].ToString() == className)
                return (Type)import[1];
        }

        return null;
    }

    public void PushLine(string line) => ExecutionQueue.Enqueue(line);

    public bool Run()
    {
        // execute the script state under a try catch so we can push the errors to the stack if any
        try
        {
            while (ExecutionQueue.Count > 0)
            {
                // execute next queued line in script
                string line = ExecutionQueue.Dequeue().Trim();

                // get regex match for class.function(arguments)
                Match match = Regex.Match(line, "^([a-zA-Z0-9]+)\\.([a-zA-Z0-9]+)\\((.*)\\)$");

                // first lets check if its a class function call using system reflection
                // then lets call the matching function in the class (via imports) with the state as argument
                if (match.Success)
                {
                    // get class name, function name and arguments from regex match
                    string className = match.Groups[1].Value;
                    string functionName = match.Groups[2].Value;
                    string arguments = match.Groups[3].Value;

                    // get class type from imports
                    Type classType = GetImport(className);

                    // check classtype if its null or not
                    if (classType == null)
                        throw new Exception($"Invalid class {className}");

                    // get function from class type
                    System.Reflection.MethodInfo method = classType.GetMethod(functionName);
                    if (method != null)
                    {
                        // push arguments to stack
                        string[] args = arguments.Split(',');
                        foreach (string arg in args)
                        {
                            string result = arg.Trim();

                            // check if argument is a string
                            if (result.StartsWith("\"") && result.EndsWith("\""))
                            {
                                // push string without quotes
                                Push(result.Substring(1, result.Length - 2), "string");
                            }
                            // check if argument is a number
                            else if (float.TryParse(result, out float number))
                            {
                                // push number
                                Push(number.ToString(), "number");
                            }
                            // check if argument is a boolean
                            else if (result == "true" || result == "false")
                            {
                                // push boolean
                                Push(result, "boolean");
                            }
                            else
                            {
                                // unknown case, lets error
                                throw new Exception($"Invalid argument {result} in {className}.{functionName}");
                            }
                        }

                        // invoke function with state as argument
                        method.Invoke(null, new object[] { this, args.Length });
                    }
                    else throw new Exception($"Invalid method {functionName} in {className}");
                }
            }
        }
        catch (Exception e)
        {
            // push error onto stack
            Push(e.Message, "string");

            // we failed to execute the script state so return false
            return false;
        }

        // we executed the script state successfully so return true
        return true;
    }

    public bool HasImport(Type thing)
    {
        // loop over each import and check if the type is already imported
        foreach (List<object> import in Imports)
        {
            if (import[1] == thing)
                return true;
        }

        return false;
    }
}