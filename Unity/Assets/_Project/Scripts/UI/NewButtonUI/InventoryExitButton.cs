using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;

public class InventoryExitButton : PlayerDisplay, ISubmitHandler, ISelectHandler, IDeselectHandler
{
    TextMeshProUGUI text;

    void Awake ()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public override void Close()
    {
        text.color = Color.white;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        text.color = Color.white;
    }

    public void OnSelect(BaseEventData eventData)
    {
        text.color = Color.magenta;
    }

    public void OnSubmit (BaseEventData baseEventData)
    {
        Inventory.instance.RemoveControl();
    }
}
