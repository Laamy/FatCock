using System;
using System.Collections.Generic;

public class FC_StateController
{
    public static void Push(FC_State state, int args)
    {
        if (state == null)
            throw new Exception("State is null (Impossible error)");
        else
        {
            // check if any arguments
            if (args == 0)
                throw new Exception("Push function takes at least 1 argument");

            // we just wont take anything off the stack
        }
    }

    public static void Pop(FC_State state, int args)
    {
        if (state == null)
            throw new Exception("State is null (Impossible error)");
        else
        {
            // check if any arguments
            if (args != 1)
                throw new Exception("Pop function takes 1 argument");

            // pop the stack the amount args[1] tells us to
            var result = state.Pop();

            // check if the argument is a number
            if (result.Key == "number")
            {
                for (int i = 0; i < args; i++)
                    _ = state.Pop(); // discard the value & key
            }
            else throw new Exception("Pop function takes a number as an argument");
        }
    }
}