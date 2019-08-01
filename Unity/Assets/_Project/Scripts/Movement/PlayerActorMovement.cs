using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerActorMovement : MonoBehaviour, ISubmitHandler
{
    [Header("Movement Values")]
    public float horizontalSpeed = 3.0f;
    public float verticalSpeed = 3.0f;

    [Header("Interaction")]
    public float interactionDistance = 2;
    public float collisionCheckDistance = 1;
    public Vector2 CollisionOffset = new Vector2(0, 0.5f);
    public LayerMask InteractionLayers;
    public LayerMask CollisionCheckLayers;
    public bool ShowDebug;

    bool useGravity = false;
    [HideInInspector] public bool UseGravity
    {
        get
        {
            return useGravity;
        }
        set
        {
            if (rb)
            {
                rb.gravityScale = value ? 4 : 0;
            }
            useGravity = value;
        }
    }

    readonly int hashLookX = Animator.StringToHash("Move X");
    readonly int hashLookY = Animator.StringToHash("Move Y");
    readonly int hashSpeed = Animator.StringToHash("Speed");

    Rigidbody2D rb;
    [Header("References")]
    public Animator animator;
    Vector2 look;
    float horizontal, vertical;
    bool safeToMove = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Debug.Assert(rb);
        //EventSystem.current.SetSelectedGameObject(gameObject);
    }

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");

            if (ShowDebug)
            {
                Debug.Log("Selected player and using XY");
            }

        }
        else
        {
            horizontal = vertical = 0;
        }
            

        if (UseGravity) vertical = 0;

        var move = new Vector2(horizontal, vertical);

        // Set our direction as long as we're not standing still
        // Otherwise, we wouldn't have a direction
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            look.Set(move.x, move.y);
            look.Normalize();
        }

        // Update look direction
        animator.SetFloat(hashLookX, look.x);
        animator.SetFloat(hashLookY, look.y);

        if (move.magnitude > 0)
        {
            // Shoot a ray in the direction we want to move. If there is something in the way, don't move there
            RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + CollisionOffset, move, collisionCheckDistance, CollisionCheckLayers.value);

            Debug.DrawRay(transform.position + (Vector3)CollisionOffset, move.normalized * collisionCheckDistance, Color.red, 0.1f);

            if (hit.collider == null)
            {
                // Update speed of our animation
                animator.SetFloat(hashSpeed, move.magnitude);
                safeToMove = true;
            }
            else
            {
                //Debug.Log(hit.collider.name);
                animator.SetFloat(hashSpeed, 0);
                safeToMove = false;
            }
        }
        else
        {
            animator.SetFloat(hashSpeed, 0);
        }
        // Always allow the player to in control when something has taken control and not given it back (by setting null)
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            UISceneManager.ClearStack();
            UISceneManager.GiveControl(PlayerReference.Player);
        }
    }

    void FixedUpdate()
    {
        if (safeToMove)
        {
            Vector2 position = rb.position;
            position.x = position.x + horizontalSpeed * horizontal * Time.deltaTime;
            position.y = position.y + verticalSpeed * vertical * Time.deltaTime;

            rb.MovePosition(position);

            if (ShowDebug)
            {
                Debug.Log("Should have moved");
            }
        }
        else if (ShowDebug)
        {
            Debug.Log("Not safe to move");
        }
    }

    public void MovePlayer (Vector2 teleportPosition, bool useGravityAtNewPos = false, bool setFogOfWar = false)
    {
        Vector3 currentPos = transform.position;
        Vector3 newPos = new Vector3(teleportPosition.x, teleportPosition.y, currentPos.z);
        transform.position = newPos;

        UseGravity = useGravityAtNewPos;
        FogOfWar.SetFogOfWar(setFogOfWar);

        UISceneManager.GiveControl(PlayerReference.Player);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        // Interactions
        // If pressed, shoot a ray in front of the player and get what we hit

        Debug.DrawRay(transform.position, look.normalized * interactionDistance, Color.blue, 1);

        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + CollisionOffset, look, interactionDistance, InteractionLayers.value);

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
