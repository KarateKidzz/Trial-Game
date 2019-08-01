using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterGuardEnemy : EnemyBehaviourBase
{
    protected override string TriggerCommand1()
    {
        string ret = "";

       
        ret = "He squints";
        CurrentDialogue = new List<string> { "I know\nyou not\nmoster.\nMe not\n stupid" };
                
        return ret;
    }
}
