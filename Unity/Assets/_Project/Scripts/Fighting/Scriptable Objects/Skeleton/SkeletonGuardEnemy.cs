using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public sealed class SkeletonGuardEnemy : EnemyBehaviourBase
{
    int numTimesComplimented;

    protected override string TriggerCommand1()
    {
        string ret = "";
        switch (numTimesComplimented)
        {
            case 0:
                ret = "His dusty bones glow a shade \nbrighter[W:1]\n\n* But he stands firm";
                CurrentDialogue = new List<string> { "A little\n compliment\n won't\n get you\n through" };
                break;

            case 1:
                ret = "He starts to think if humans are as\n bad as he thought";
                CurrentDialogue = new List<string> { "That was\n.[W:0.4].[W:0.4].[W:0.4]\n nic-", "I'm not\n letting\n you\n through!" };
                break;

            default:
                ret = "He can't imagine hurting something as\n kind as you and doesn't want\n to fight anymore";
                CurrentDialogue = new List<string> { "Humans\n aren't meant\n to be\n nice!", "But...\n your words\n are \nappreciated" };
                CanSpare = true;
                break;
        }
        numTimesComplimented++;
        return ret;
    }

    protected override string TriggerCommand2()
    {
        switch (numTimesComplimented)
        {
            case 0:
            case 1:
            case 2:
                CurrentDialogue = new List<string> { "I knew humans weren't to be trusted!" };
                break;

            default:
                CurrentDialogue = new List<string> { ".[W:0.5].[W:0.5].[W:0.5]\nAfter all\n the things\n you said..." };
                break;
        }
        if (numTimesComplimented > 0) numTimesComplimented--;
        Defense++;
        return "It did not like that. Enemy DEF +1";
    }
}
