using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerInventory : ScriptableObject
{
    static PlayerInventory instance;

    public List<BaseCollectable> Items = new List<BaseCollectable>();

    [RuntimeInitializeOnLoadMethod]
    public static void Init ()
    {
        instance = Resources.LoadAll<PlayerInventory>("")[0];
        instance.Items.Clear(); // Clear the inventory on each game session
        Debug.Log("[Player Inventory] Initialised");
    }

    public static List<BaseCollectable> GetItems ()
    {
        Debug.Assert(instance != null);
        return instance.Items;
    }
}
