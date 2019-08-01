using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// When active, moves the actor. Activation controlled by <see cref="Actor"/>
/// </summary>
public abstract class Controller : MonoBehaviour
{
    protected Actor Actor { get; set; }

    readonly int hashLookX = Animator.StringToHash("Move X");
    readonly int hashLookY = Animator.StringToHash("Move Y");
    readonly int hashSpeed = Animator.StringToHash("Speed");

    protected Rigidbody2D MainRigidbody;
    protected Animator Animator;

    protected Vector2 Move;
    bool safeToMove = true;

    /// <summary>
    /// Called when the controller is first added to the actor and activated
    /// </summary>
    /// <param name="pawn">Pawn.</param>
    public void OnControllerEnable (Actor pawn, Rigidbody2D rb, Animator a)
    {
        Actor = pawn;
        MainRigidbody = rb;
        Animator = a;

        OnControllerEnableHook();
    }

    protected virtual void OnControllerEnableHook ()
    {
        
    }

    /// <summary>
    /// Called when the controller has lost control and has been removed from the actor 
    /// </summary>
    public virtual void OnControllerDisable ()
    {
        
    }

    public virtual void OnControllerPause ()
    {
        
    }

    /// <summary>
    /// Update proxy
    /// </summary>
    public void ControllerUpdate ()
    {
        Debug.Assert(Actor != null);
        if (Actor == null) return;

        DoMovement();

        if (Actor.UseGravity) Move.Set(Move.x, 0);

        // Set our direction as long as we're not standing still
        // Otherwise, we wouldn't have a direction
        if (!Mathf.Approximately(Move.x, 0.0f) || !Mathf.Approximately(Move.y, 0.0f))
        {
            Actor.Look.Set(Move.x, Move.y);
            Actor.Look.Normalize();
        }

        // Update look direction
        Animator.SetFloat(hashLookX, Actor.Look.x);
        Animator.SetFloat(hashLookY, Actor.Look.y);

        if (Move.magnitude > 0)
        {
            // Shoot a ray in the direction we want to move. If there is something in the way, don't move there
            RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + Actor.CollisionCheckOffset, Move, Actor.CollisionCheckDistance, Actor.CollisionCheckLayers.value);

            Debug.DrawRay(transform.position + (Vector3)Actor.CollisionCheckOffset, Move.normalized * Actor.CollisionCheckDistance, Color.red, 0.1f);

            if (hit.collider == null)
            {
                // Update speed of our animation
                Animator.SetFloat(hashSpeed, Move.magnitude);
                safeToMove = true;
            }
            else
            {
                //Debug.Log(hit.collider.name);
                Animator.SetFloat(hashSpeed, 0);
                safeToMove = false;
            }
        }
        else
        {
            Animator.SetFloat(hashSpeed, 0);
        }
    }

    /// <summary>
    /// Called when the gameobject is not selected (like being in the UI)
    /// </summary>
    public void ControllerUnselected ()
    {
        Move.Set(0, 0);
        Animator.SetFloat(hashSpeed, 0);
    }

    /// <summary>
    /// Fixed Update proxy
    /// </summary>
    public void ControllerFixedUpdate ()
    {
        Debug.Assert(Actor != null);
        if (Actor == null) return;

        if (safeToMove)
        {
            Vector2 position = MainRigidbody.position;
            position.x = position.x + Actor.HorizontalSpeed * Move.x * Time.deltaTime;
            position.y = position.y + Actor.VerticalSpeed * Move.y * Time.deltaTime;

            MainRigidbody.MovePosition(position);
        }
    }

    /// <summary>
    /// Called before update. Allows the controller to set movement values without worrying about movement implementation
    /// </summary>
    public abstract void DoMovement();

    public virtual void OnSubmit() { }
}
