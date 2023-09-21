using System;
using System.Collections.Generic;

public class Headers
{
    // define a list of headers with their assigned C# class
    public static Dictionary<string, Type> headers = new Dictionary<string, Type>()
    {
        { "FC_Console", typeof(FC_Console) },
        { "FC_Threading", typeof(FC_Thread) },
        { "FC_StateController", typeof(FC_StateController) }
    };

    // get the defined class of a header, else return null
    public static Type GetHeader(string header)
    {
        // compare with headers in the dictionary and return the class if it exists
        if (headers.ContainsKey(header))
        {
            // return the class
            return headers[header];
        }
        else
        {
            // return null
            return null;
        }
    }
}