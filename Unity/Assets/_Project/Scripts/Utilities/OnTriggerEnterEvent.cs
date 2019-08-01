using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class OnTriggerEnterEvent : MonoBehaviour
{
    public event Action<OnTriggerEnterEvent> OnTriggerEnter;
    public LayerMask TriggerMask;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & TriggerMask) != 0)
        {
            OnTriggerEnter?.Invoke(this);
        }
    }
}
