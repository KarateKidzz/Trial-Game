using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectableUI : MonoBehaviour
{
    public Image Image;
    public string ItemName;
    public BaseCollectable Prefab;

    public CollectableUI (string name, Sprite icon, BaseCollectable prefab)
    {
        ItemName = name;
        Image.sprite = icon;
        Prefab = prefab;
    }

    void Start()
    {
        ItemName = GetComponentInChildren<TMPro.TextMeshProUGUI>().text;
    }

    void OnDisable()
    {
        OnMouseExit();
    }

    public void OnMouseEnter()
    {
        Color orig = Image.color;
        Image.color = new Color(orig.r, orig.g, orig.b, 0.6f);
    }

    public void OnMouseExit()
    {
        Color orig = Image.color;
        Image.color = new Color(orig.r, orig.g, orig.b, 0);
    }

    public void OnMouseDown()
    {
        Inventory.OnItemSelect(Prefab);
    }
}
