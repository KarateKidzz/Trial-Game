using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
public class BaseInteractable : MonoBehaviour, ISelectHandler
{
    public bool Interactable = true;
    public bool InteractOnTriggerEnter;
    public bool TriggerOnce;
    public UnityEvent OnInteract;

    bool triggered;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (InteractOnTriggerEnter)
        {
            if (!TriggerOnce || (TriggerOnce && !triggered))
            {
                Trigger();
                if (TriggerOnce)
                {
                    triggered = true;
                }
            }

        }
    }

    public virtual void Trigger ()
    {
        if (Interactable)
        {
            OnInteract.Invoke();
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        Trigger();
    }

    public void SetInteractable(bool value)
    {
        Interactable = value;
    }
}
