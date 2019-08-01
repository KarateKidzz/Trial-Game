using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OnUseMethod
{
    public BaseCollectable Collectable;
    public UnityEngine.Events.UnityEvent Method;
}

/// <summary>
/// Allows a collectable to be used on this object
/// </summary>
public class InteractableWithCollectable : InteractableWithOptions
{
    public List<OnUseMethod> InteractionOptions = new List<OnUseMethod>();

    public bool UseCollectable (BaseCollectable collectable)
    {
        for (int i = 0; i < InteractionOptions.Count; i++)
        {
            if (InteractionOptions[i].Collectable.Name == collectable.Name)
            {
                InteractionOptions[i].Method.Invoke();
                return true;
            }
        }
        return false;
    }
}
