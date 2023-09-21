using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class FCParser
{
    // tokenize string (unfinished)
    public static List<string> Tokenize(string code)
    {
        // split script into its individual tokens
        string pattern = @"(\{|}|\(|\)|.|@|\s)"; // tokenize pattern
        string[] tokens = Regex.Split(code, pattern);

        // remove empty tokens
        List<string> filteredTokens = new List<string>();

        foreach (string token in tokens)
        {
            if (token.Trim().Length > 0)
                filteredTokens.Add(token);
        }

        // return filtered tokens
        return filteredTokens;
    }

    // unfinished additional parsing state enum
    public static FC_State ParseLine(string code)
    {
        // tokenize the code
        List<string> tokens = Tokenize(code);
        
        // create a new fc state
        FC_State state = new FC_State();

        // current state (feel free to make up states and i'll add them as we go)
        ParsingState currentState = ParsingState.Idle;

        Stack<string> stack = new Stack<string>();

        // loop over each token and update the next token state enum
        foreach (string token in tokens)
        {
            switch (currentState)
            {
                case ParsingState.Idle:
                    {
                        // check if the next token is an import
                        if (token == "import")
                        {
                            // set the current state to import
                            currentState = ParsingState.Import;
                        }
                        else
                        {

                        }
                    }
                    break;

                case ParsingState.Import:
                    {
                        if (token == "as")
                        {
                            // we're gonna import something "AS" something else
                            currentState = ParsingState.ImportAs;
                        }
                        else throw new Exception($"Unknown token {token}");
                    }
                    break;

                case ParsingState.ImportAs: // type of import is "as"
                    {
                        // make sure token is only A-Z, a-Z, _ (else throw error)
                        if (!Regex.IsMatch(token, @"^[a-zA-Z_]+$"))
                            throw new Exception($"Invalid identifier {token}");

                        // push class name to run stack
                        stack.Push(token);

                        // set the current state to importAsIdentifier
                        currentState = ParsingState.ImportAsIdentifier;
                    }
                    break;

                case ParsingState.ImportAsIdentifier: // type of import is "as" and we have an identifier
                    {
                        if (token == "from")
                        {
                            // we're gonna import something "AS" something else "FROM" somewhere
                            currentState = ParsingState.ImportAsIdentifierFrom;
                        }
                        else throw new Exception($"Unknown token {token}");
                    }
                    break;

                case ParsingState.ImportAsIdentifierFrom:
                    {
                        // get "thing" & "header" from stack & current token
                        string thing = stack.Pop();
                        string header = token;

                        // first compare to inbuilt api headers
                        Type type = Headers.GetHeader(header);

                        // if the header is not in the inbuilt api headers, check the user headers
                        // TODO: Implement user headers

                        // if the header is not in the user headers, throw an error
                        if (type == null)
                            throw new Exception($"Header {header} not found");

                        // check for duplicate imports
                        if (state.HasImport(type))
                            throw new Exception($"Duplicate import {type}");

                        // finally add the header to the state
                        state.PushImport(new List<object> { thing, type });

                        // set the current state to idle
                        currentState = ParsingState.Idle;
                    }
                    break;
            }
        }

        return state;
    }

    // parse using old regex parsing
    public static FC_State Parse(string code)
    {
        // create a new fc state
        FC_State state = new FC_State();

        // split script into its individual lines
        string[] lines = code.Split('\n');

        // loop over each line and run some basic test parsing for this prototype
        foreach (string line in lines)
        {
            // handle import statements (Import THING from HEADER)
            Match match = Regex.Match(line, @"import\sas\s+""(.*?)""\s+from\s+""(.*?)""");
            if (match.Success)
            {
                // thing & header from regex to strings from reg (Match)
                string thing = match.Groups[1].Value;
                string header = match.Groups[2].Value;

                // first compare to inbuilt api headers
                Type type = Headers.GetHeader(header);

                // if the header is not in the inbuilt api headers, check the user headers
                // TODO: Implement user headers

                // if the header is not in the user headers, throw an error
                if (type == null)
                    throw new Exception($"Header {header} not found");

                // check for duplicate imports
                if (state.HasImport(type))
                    throw new Exception($"Duplicate import {type}");

                // finally add the header to the state
                state.PushImport(new List<object> { thing, type });
            }
            else
            {
                // check if line is only empty (whitespaces or tabs)
                if (line.Trim().Length == 0)
                    continue;

                // add the line to the state for execution
                state.PushLine(line);
            }
        }

        // return hopefully complete fc script state
        return state;
    }
}