using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class PlayerDefaultController : Controller
{
    public Vector2 InteractionOffset = new Vector2(0, 0.5f);
    public LayerMask InteractionLayers;
    public float interactionDistance = 2;

    public override void DoMovement()
    {
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            Move.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (Input.GetKeyDown(KeyCode.C))
            {
                Inventory.Toggle();
            }
        }
        else
        {
            Move.Set(0, 0);
        }
    }

    public override void OnControllerPause()
    {
        Inventory.StaticRemoveControl();
    }


    public override void OnSubmit()
    {
        Debug.DrawRay(transform.position, Actor.Look.normalized * interactionDistance, Color.blue, 1);

        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + InteractionOffset, Actor.Look, interactionDistance, InteractionLayers.value);

        if (hit.collider != null)
        {
            //Debug.Log(hit.collider.name);

            BaseCollectable collectable = hit.collider.GetComponentInChildren<BaseCollectable>();
            if (collectable)
            {
                collectable.PickUp();
            }

            BaseInteractable interactable = hit.collider.GetComponentInChildren<BaseInteractable>();
            if (interactable != null)
            {
                //EventSystem.current.SetSelectedGameObject(interactable.gameObject);
                interactable.Trigger();
            }
        }
    }
}
