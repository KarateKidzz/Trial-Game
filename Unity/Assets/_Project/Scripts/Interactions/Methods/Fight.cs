using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Fight : MonoBehaviour
{
    public EncounterBehaviourBase Prefab;
    public UnityEvent OnBattleEnd;
    
    public void StartFight ()
    {
        CurrentEncounter.SetEncounter(Prefab, transform.parent.gameObject, OnBattleEnd.Invoke);
        UIGroupManager.CloseDisplays();
        SceneHelper.LoadBattleScene();
    }
}
