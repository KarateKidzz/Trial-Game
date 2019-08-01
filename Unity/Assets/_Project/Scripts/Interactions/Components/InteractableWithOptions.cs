using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[System.Serializable]
public class InteractableOption
{
    public string OptionName;
    public UnityEvent OptionEvent;
}

public class InteractableWithOptions : BaseInteractable
{
    [TextArea]
    public string Text;
    public InteractableOption[] Options = new InteractableOption[3];

    public override void Trigger ()
    {
        if (Interactable)
            InteractableText.DisplayOptions(Text, Options);
    }

}
