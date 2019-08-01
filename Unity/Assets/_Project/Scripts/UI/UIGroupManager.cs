using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Allows UI classes to be managed by a manager
/// </summary>
[Serializable]
public class PlayerDisplay : MonoBehaviour
{
    /// <summary>
    /// Shows this object's UI
    /// </summary>
    public virtual void Open() { }

    /// <summary>
    /// Hides this object's UI. This doesn not mean it gives up control
    /// </summary>
    public virtual void Close() { }

    /// <summary>
    /// Gives control to this UI element and shows it
    /// </summary>
    public void TakeControl(bool leavePreviousOpen = false)
    {
        UISceneManager.GiveControl(this, leavePreviousOpen);
    }

    /// <summary>
    /// Tells the manager to close this object and any object higher than it on the stack
    /// </summary>
    public void RemoveControl () 
    {
        UISceneManager.RemoveControl(this);
    }
}

public class UIGroupManager : MonoBehaviour
{
    public bool StartAllChildrenHidden = true;

    /// <summary>
    /// Displays that shouldn't display at the same time
    /// </summary>
    public List<PlayerDisplay> ChildDisplays = new List<PlayerDisplay>();

    /// <summary>
    /// List all of managers in the scene
    /// </summary>
    static readonly List<UIGroupManager> managers = new List<UIGroupManager>();

    void Awake()
    {
        if (!managers.Contains(this))
        {
            managers.Add(this);
        }
    }

    void OnDestroy()
    {
        if (managers.Contains(this))
        {
            managers.Remove(this);
        }
        Time.timeScale = 1;
    }

    void Start()
    {
        if (StartAllChildrenHidden)
        {
            CloseDisplays();
        }
    }

    public void CloseGlobalDisplays ()
    {
        CloseDisplays();

    }

    public static void CloseDisplays ()
    {
        for (int m = 0; m < managers.Count; m++)
        {
            for (int i = 0; i < managers[m].ChildDisplays.Count; i++)
            {
                managers[m].ChildDisplays[i].Close();
            }
        }
        Time.timeScale = 1;
        if (!EventSystem.current.alreadySelecting)
            EventSystem.current.SetSelectedGameObject(PlayerReference.Player.gameObject);
    }

    public static void PauseGame ()
    {
        Time.timeScale = 0;
    }
}
