using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Inventory : PlayerDisplay
{
    public GameObject RootObject;
    public GameObject ListObject;
    public GameObject ItemPrefab;

    public PlayerDisplay ExitButton;

    public static Inventory instance;
    public static GameObject ListRoot => instance.ListObject;

    public List<BaseCollectable> Items => PlayerInventory.GetItems();

    // If the game has requested the player to choose an item, this should be set to true to allow selecting an item
    static InteractableWithCollectable interactingObject = null;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        for (int i = 0; i < instance.Items.Count; i++)
        {
            GameObject newItemObject = Instantiate(instance.ItemPrefab, instance.ListObject.transform, false);
            newItemObject.transform.GetChild(0).GetComponent<Image>().sprite = instance.Items[i].Icon;
            newItemObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText(instance.Items[i].Name);
            newItemObject.GetComponent<CollectableUI>().Prefab = instance.Items[i];
        }
    }

    public override void Close ()
    {
        RootObject.SetActive(false);
        interactingObject = null;
    }

    public override void Open ()
    {
        RootObject.SetActive(true);
        ExitButton.TakeControl(true);
    }

    public void OpenWithInteractable (InteractableWithCollectable interactable)
    {
        interactingObject = interactable;
        TakeControl(true);
    }

    public static void AddItem (BaseCollectable collectable)
    {
        instance.Items.Add(collectable);

        GameObject newItemObject = Instantiate(instance.ItemPrefab, instance.ListObject.transform, false);

        newItemObject.transform.GetChild(0).GetComponent<Image>().sprite = collectable.Icon;
        newItemObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText(collectable.Name);
        newItemObject.GetComponent<CollectableUI>().Prefab = collectable;
    }

    public static void OnItemSelect (BaseCollectable item)
    {
        if (interactingObject != null)
        {
            for (int i = 0; i < instance.Items.Count; i++)
            {
                BaseCollectable current = instance.Items[i];
                if (current == item)
                {
                    if (interactingObject.UseCollectable(current))
                    {
                        UISceneManager.GiveControl(PlayerReference.Player);
                    }
                    return;
                }
            }
        }
    }

    public static void StaticRemoveControl ()
    {
        instance.Close();
    }

    public static void Toggle ()
    {
        instance.toggle();
    }

    void toggle()
    {
        if (RootObject.activeInHierarchy)
            RemoveControl();
        else
            TakeControl();
    }
}
