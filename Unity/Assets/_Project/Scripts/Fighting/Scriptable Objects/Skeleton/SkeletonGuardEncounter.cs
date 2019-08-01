using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonGuardEncounter : EncounterBehaviourBase
{
    // Meant to be the tutorial wave
    // Waves after that will be random
    public EncounterWaveBase BasicWave;

    public override EncounterState EncounterStart()
    {
        return EncounterState.PreActionDialogue;
    }

    public override void EnemyDialogueEnding()
    {
        if (NumberOfDefenseRounds == 0)
            CurrentWave = BasicWave;
        else
            base.EnemyDialogueEnding();
    }
}
