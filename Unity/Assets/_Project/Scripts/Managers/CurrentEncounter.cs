using System;
using UnityEngine;

[CreateAssetMenu]
public class CurrentEncounter : ScriptableObject
{
    static SceneHelper instance;

    public static EncounterBehaviourBase CurrentEcounterBehaviour { get; private set; }
    public static GameObject EncounterStarter { get; private set; }
    public static Action OnWin { get; private set; }

    [RuntimeInitializeOnLoadMethod]
    public static void Init()
    {
        instance = Resources.LoadAll<SceneHelper>("")[0];
        Debug.Log("[Current Encounter] Initialised");
    }

    public static void SetEncounter (EncounterBehaviourBase behaviourBase, GameObject caller, Action onWinAction)
    {
        CurrentEcounterBehaviour = behaviourBase;
        EncounterStarter = caller;
        OnWin = onWinAction;
    }

    /// <summary>
    /// Trigger an action that will update the Overworld scene to reflect the changes in the fight. This will not destory the calling enemy
    /// </summary>
    public static void TriggerWin ()
    {
        if (OnWin != null)
        {
            OnWin.Invoke();
        }
    }

    public static void DestoryCaller ()
    {
        if (EncounterStarter != null)
        {
            Destroy(EncounterStarter);
        }
        else
        {
            Debug.LogWarning("[Current Encounter] The Object that started the encounter can not be found and will not be destroyed");
        }
    }
}
