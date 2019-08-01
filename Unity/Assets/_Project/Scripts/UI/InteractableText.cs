using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.EventSystems;

public class InteractableText : PlayerDisplay
{
    public GameObject Parent;
    public InteractiveTextOption[] Options = new InteractiveTextOption[3];
    public TextMeshProUGUI BodyText;
    public TextMeshProUGUI[] OptionTexts = new TextMeshProUGUI[3];
    readonly UnityEvent[] OptionEvents = new UnityEvent[3];

    static InteractableText instance;

    void Awake()
    {
        instance = this;
    }

    // ----- PlayerDisplay Overriding ----- //

    public override void Open()
    {
        Parent.SetActive(true);
        Time.timeScale = 0;
    }

    public override void Close()
    {
        Parent.SetActive(false);
        Time.timeScale = 1;
    }

    // When given control back from options, remove control ourself as we've completed our task and should close
    public static void StaticTakeControl() => instance.RemoveControl();

    // ----- PlayerDisplay Overriding ----- //

    public static void DisplayOptions (string bodyText, InteractableOption[] interactableOptions)
    {
        Debug.Assert(interactableOptions != null && interactableOptions.Length <= 3 && interactableOptions.Length > 0, "[Interactable Text] Invalid Options", instance.gameObject);

        // Take control
        // We wil give control to specific options later
        // Also, by having control now, we can also 'go back' to this
        instance.TakeControl();

        instance.BodyText.SetText(bodyText);

        // Looop through parameter options and set the UI
        for (int i = 0; i < instance.OptionTexts.Length; i++)
        {
            // Get current. While still inside array, return index, else return null
            // If null, turn of and set text off, else set UI
            InteractableOption current = interactableOptions.Length > i ? interactableOptions[i] : null;
            if (current != null)
            {
                instance.Options[i].gameObject.SetActive(true);
                instance.Options[i].Text.text = current.OptionName;
                instance.Options[i].OnClick = current.OptionEvent;

            }
            else
            {
                instance.Options[i].gameObject.SetActive(false);
                instance.Options[i].Text.text = "";
                instance.Options[i].OnClick = null;
            }
        }
        // Give control to first option
        instance.Options[0].TakeControl(true);
    }
}
