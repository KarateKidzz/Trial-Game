using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInteraction : MonoBehaviour, ISubmitHandler
{
    Actor actor;

    public Vector2 InteractionOffset = new Vector2(0, 0.5f);
    public LayerMask InteractionLayers;
    public float interactionDistance = 2;

    void Awake ()
    {
        actor = GetComponent<Actor>();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        // Interactions
        // If pressed, shoot a ray in front of the player and get what we hit

        Debug.DrawRay(transform.position, actor.Look.normalized * interactionDistance, Color.blue, 1);

        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + InteractionOffset, actor.Look, interactionDistance, InteractionLayers.value);

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
