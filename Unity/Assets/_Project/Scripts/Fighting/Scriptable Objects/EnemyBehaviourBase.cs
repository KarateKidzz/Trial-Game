using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ItemAction
{
    public BaseCollectable Item;
    [TextArea]
    public string Text;
    public bool CanSpare;
}

public enum TextBubbleDirection
{
    Left,
    LeftShort,
    LeftWide,
    Right,
    RightLarge,
    RightLong,
    RightShort,
    RightWide,
    Top,
    TopTiny,
    Bottom
}

public class EnemyBehaviourBase : MonoBehaviour
{
    public string EnemyName = "Enemy";
    public Sprite Sprite;
    public TextBubbleDirection TextBubbleDirection = TextBubbleDirection.Right;
    public int HealthPoints = 100;
    [HideInInspector] public int StartHealth;
    public int Attack = 1;
    public int Defense = 1;
    public string CheckMessage = "The enemy looks like itself";
    public bool CanCheck = true;
    public bool CanSpare;
    [ReadOnly]
    public List<string> CurrentDialogue = new List<string>();

    public List<string> PreActionDialogue = new List<string>();
    public List<string> Comments = new List<string>();   // Things the enemy can say at the start of dialogue in the main widget
    public List<string> RandomDialogue = new List<string>();
    public List<string> Commands = new List<string>(1);   // Options the player can click on
    public List<ItemAction> ItemActions;

    void Awake()
    {
        StartHealth = HealthPoints;
    }

    public bool IsDead ()
    {
        return HealthPoints <= 0;
    }

    public string TriggerCommandEvent (int command)
    {
        string ret = "";

        switch (command - 1)
        {
            case 1:
                ret = TriggerCommand1();
                break;

            case 2:
                ret = TriggerCommand2();
                break;

            case 3:
                ret = TriggerCommand3();
                break;
        }
        return ret;
    }

    protected virtual string TriggerCommand1 ()
    {
        return "Action 1 Return";
    }

    protected virtual string TriggerCommand2()
    {
        return "Action 2 Return";
    }

    protected virtual string TriggerCommand3()
    {
        return "Action 3 Return";
    }

    /// <summary>
    /// Returns true if the enemy is affected by the item, ie. using glasses makes a scientist stop attacking you. Else, returns false and the item's effect is applied to the player (if any)
    /// </summary>
    /// <returns><c>true</c>, if item was handled, <c>false</c> otherwise.</returns>
    /// <param name="item">Item.</param>
    public string HandleItem (BaseCollectable item)
    {
        Debug.Log("Handle Item");
        foreach (ItemAction i in ItemActions)
        {
            Debug.Log(i.Item.Name);
            Debug.Log(item.Name);
            Debug.Log("F");
            if (i.Item == item)
            {
                // Give no oppurtunity to set to false, otherwise an item may allow sparing, and another turns it off when we don't want it to
                if (i.CanSpare)
                {
                    CanSpare = true;
                }
                Debug.Log("Found Item");
                return i.Text;
            }
        }
        return "";
    }
}
