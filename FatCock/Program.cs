using System;
using System.IO;

public class Program
{
    static void Main(string[] args)
    {
        // read test .fc script from file (example.fc)
        string code = File.ReadAllText("example.fc");

        // parse the script into a state which we can then execute line by line
        FC_State state = FCParser.Parse(code);

        // execute the parsed state
        if (!state.Run())
        {
            // pop error from stack (errors are strings regardless)
            string err = state.Pop().Value;

            // print error
            Console.WriteLine(err);
            Console.ReadKey();
        }
    }
}