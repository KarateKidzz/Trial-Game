using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputHelp
{
    /// <summary>
    /// Is the confirm key pressed?
    /// </summary>
    /// <returns>The confirm.</returns>
    public static bool Confirm ()
    {
        return Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return);
    }

    public static bool AnyArrorKey ()
    {
        return Input.GetKeyDown(KeyCode.LeftApple) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow);
    }
}
