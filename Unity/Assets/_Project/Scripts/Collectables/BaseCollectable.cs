using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class BaseCollectable : MonoBehaviour
{
    public string Name = "Inventory Item";
    public Sprite Icon;

    public UnityEvent OnPickup;

    public virtual void PickUp()
    {
        Debug.Log("Pick up");
        GameObject playerObject = PlayerReference.Player.gameObject;
        transform.parent = playerObject.transform;
        gameObject.SetActive(false);

        Inventory.AddItem(this);
    }

    public virtual void Use()
    {

    }

    void OnMouseDown()
    {
        PickUp();
    }

    public static bool operator == (BaseCollectable one, BaseCollectable two)
    {
        return one.Name == two.Name && one.Icon == two.Icon;
    }

    public static bool operator != (BaseCollectable one, BaseCollectable two)
    {
        return one.Name != two.Name || one.Icon != two.Icon;
    }

    public override bool Equals(object obj)
    {
        var collectable = obj as BaseCollectable;
        return collectable != null &&
               base.Equals(obj) &&
               Name == collectable.Name &&
               EqualityComparer<Sprite>.Default.Equals(Icon, collectable.Icon);
    }

    public override int GetHashCode()
    {
        var hashCode = -1564689140;
        hashCode = hashCode * -1521134295 + base.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
        hashCode = hashCode * -1521134295 + EqualityComparer<Sprite>.Default.GetHashCode(Icon);
        return hashCode;
    }
}
