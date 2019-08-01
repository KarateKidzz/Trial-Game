using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Events;
using System;

public class InteractiveTextOption : PlayerDisplay, ISubmitHandler, ISelectHandler, IDeselectHandler, IUpdateSelectedHandler
{
    public TextMeshProUGUI Text;
    [ReadOnly]
    public UnityEvent OnClick;
    public PlayerDisplay Left, Right;

    public override void Close()
    {
        Text.color = Color.white;
        Text.fontStyle = FontStyles.Normal;
    }

    public void OnSelect(BaseEventData eventData)
    {
        Text.color = Color.magenta;
        Text.fontStyle = FontStyles.Underline;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        Text.color = Color.white;
        Text.fontStyle = FontStyles.Normal;
    }

    public void OnSubmit(BaseEventData eventData)
    {
        // Invoke our method
        if (OnClick != null)
        {
            OnClick.Invoke();
        }
        // Give control back main
        //InteractableText.StaticTakeControl();
    }

    public void OnUpdateSelected(BaseEventData eventData)
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && Left != null && Left.isActiveAndEnabled)
        {
            Left.TakeControl();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && Right != null && Right.isActiveAndEnabled)
        {
            Right.TakeControl();
        }
    }
}
