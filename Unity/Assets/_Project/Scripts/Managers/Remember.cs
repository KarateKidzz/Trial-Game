using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Remebers basic things through the lifetime of the game
/// </summary>
[CreateAssetMenu]
public class Remember : ScriptableObject
{
    static Remember Instance;

    /// <summary>
    /// If true, on game start, the instructions screen will show before the menu
    /// </summary>
    public bool showInstructions;

    [RuntimeInitializeOnLoadMethod]
    public static void Init ()
    {
        Instance = Resources.LoadAll<Remember>("")[0];
        Debug.Log("[Memory] Initialised");
    }

    public static bool ShowInstructions
    {
        get => Instance.showInstructions;
        set => Instance.showInstructions = value;
    }
}
