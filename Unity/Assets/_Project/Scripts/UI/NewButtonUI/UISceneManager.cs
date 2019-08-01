using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*
    The original UI was set up for mouse, not keyboard. This meant you could always get out of a selection due
    to having mouse control and being able to click anywhere on screen. However, when moving to keyboard,
    it became the code's responsability to handle who had control.

    The setup became hacky. Each UI element had control and would have to (itself) remember what to give control back to.
    This did work, but if you lost selection (for instance, coming out of the game), the code would default back to the player,
    even if the UI is open. This caused bugs and stopped people from continuing.

    To fix this, UISceneManager is made.

    The goal is to have a Stack of selectable items. The player should add itself first, and everything else is added on top.
    When that object doesn't need control, it pops off the stack and gives control back to the last selected object. Popping can 
    keep happening until we reach the player where there would be no reason to pop any further.

    A selectable object should then have public functions like 'OnConfirm' and 'OnCancel' to allow popping off the stack or selecting
    a child object (for instance, the inventory is a selectable object but it gives control over to individual elements, while still being
    able to exit).

*/

/*
    It is important to clear the stack on each scene.

    when the player from the new scene takes control, it is added to the top (and doesn't replace the bottom player).
    By clearing, we allow the new player to be the 'bottom' and have all new selections go above without increasing the stack size
    massively.
*/

/// <summary>
/// Handles selecting and deselecting UI elements
/// </summary>
public static class UISceneManager
{
    static EventSystem EventSystem => EventSystem.current;

    static Stack<PlayerDisplay> Displays = new Stack<PlayerDisplay>();

    public static void GiveControlToNonPlayerDisplay (GameObject gameObject)
    {
        CoroutineManager.StartRoutine(GiveControlWait(gameObject));
    }

    /// <summary>
    /// Closes and removes all objects from the stack, allowing the next object added to be the base
    /// </summary>
    public static void ClearStack ()
    {
        while (Displays.Count > 0)
        {
            Displays.Pop()?.Close();
        }
    }

    /// <summary>
    /// Select the last selected object
    /// </summary>
    public static void SelectLast ()
    {
        if (Displays.Count > 0)
        {
            CoroutineManager.StartRoutine(GiveControlWait(Displays.Peek().gameObject));
        }
    }

    /// <summary>
    /// Adds and gives control to <paramref name="playerDisplay"/>. If it is already in the stack, it will close all displays above it, rather than add it anew
    /// </summary>
    /// <param name="playerDisplay">Player display.</param>
    public static void GiveControl (PlayerDisplay playerDisplay, bool leavePreviousOpen = false)
    {
        CoroutineManager.StartRoutine(GiveControlWait(playerDisplay.gameObject));

        // If there's no items in the stack we can push straight away
        if (Displays.Count == 0)
        {
            Displays.Push(playerDisplay);
            playerDisplay.Open();
            return;
        }

        // If the item isn't in the stack, we can add it straight to the top
        if (!Displays.Contains(playerDisplay))
        {
            // Close UI of the current selected
            PlayerDisplay currentTop = Displays.Peek();
            if (!leavePreviousOpen)
                currentTop.Close();

            // Add our new object to the top and show it
            Displays.Push(playerDisplay);
            playerDisplay.Open();
        }
        // Else, remove all items on top
        else
        {
            // Loop through top most items and close/remove them
            while (Displays.Peek() != playerDisplay && Displays.Count > 0)
            {
                PlayerDisplay currentTop = Displays.Pop();
                if (!leavePreviousOpen)
                    currentTop.Close();
            }
            // Top most must be the object we're after so show it
            PlayerDisplay top = Displays.Peek();
            top.Open();
        }
    }

    /// <summary>
    /// Closes and removes control for <paramref name="playerDisplay"/>. If the <paramref name="playerDisplay"/> is not the topmost object, all objects above it will be also be closed and removed
    /// </summary>
    /// <param name="playerDisplay">Player display.</param>
    public static void RemoveControl (PlayerDisplay playerDisplay)
    {

        if (Displays.Count == 0) return;

        do
        {
            // Close UI at the top
            Displays.Peek().Close();
        }
        // Keep removing until the thing we removed was the one we need to remove
        while (Displays.Pop() != playerDisplay && Displays.Count > 0);

        // Give control back to the next object down
        Displays.Peek().Open();

        // Give control
        CoroutineManager.StartRoutine(GiveControlWait(Displays.Peek().gameObject));
    }

    static IEnumerator GiveControlWait (GameObject playerDisplayGameObject)
    {
        while (EventSystem.alreadySelecting) yield return null;
        EventSystem.SetSelectedGameObject(playerDisplayGameObject);
        yield return null;
    }
}
