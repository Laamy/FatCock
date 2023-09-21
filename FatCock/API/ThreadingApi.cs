using System;
using System.Threading;

public class ThreadingApi
{
    // sleep in seconds
    public static void Sleep(FC_State state, int args)
    {
        if (args == 1)
        {
            var result = state.Pop();

            if (result.Key == "number")
            {
                // get the time from the state
                int time = int.Parse(result.Value) * 1000;

                // sleep
                Thread.Sleep(time);
            }

            else throw new Exception("Sleep function takes a number as an argument");
        }
        else throw new Exception("Sleep function takes 1 argument");
    }
}