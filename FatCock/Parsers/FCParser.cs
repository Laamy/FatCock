using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class FCParser
{
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