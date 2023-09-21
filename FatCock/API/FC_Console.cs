using System;

public class FC_Console
{
    // write to console
    public static void Write(FC_State state, int args)
    {
        if (args == 1)
        {
            var result = state.Pop();

            if (result.Key == "string")
            {
                // get the message from the state
                string message = result.Value;

                // print the message
                Console.Write(message);
            }
            else throw new Exception("Write function takes a string as an argument");
        }
        else throw new Exception("Write function takes 1 argument");
    }

    // write to console with new line
    public static void WriteLine(FC_State state, int args)
    {
        if (args == 1)
        {
            var result = state.Pop();

            if (result.Key == "string")
            {
                // get the message from the state
                string message = result.Value;

                // print the message
                Console.WriteLine(message);
            }
            else throw new Exception("WriteLine function takes a string as an argument");
        }
        else throw new Exception("WriteLine function takes 1 argument");
    }

    // clear console
    public static void Clear(FC_State state, int args)
    {
        if (args == 0)
        {
            // clear console
            Console.Clear();
        }
        else throw new Exception("Clear function takes 0 arguments");
    }
}