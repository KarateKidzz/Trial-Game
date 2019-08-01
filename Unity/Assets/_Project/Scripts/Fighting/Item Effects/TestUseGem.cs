using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUseGem : MonoBehaviour
{
    public void UseGem (EncounterBehaviourBase info)
    {
        info.Enemies[0].CanSpare = true;
        info.EncounterText = "The monster.[W:0.2].[W:0.2].[W:0.2] appreciated that";
    }
}
