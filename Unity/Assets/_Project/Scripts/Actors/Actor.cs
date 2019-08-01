using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Data container about the actor
/// </summary>
public class Actor : MonoBehaviour, ISubmitHandler
{
    [Header("Movement Values")]
    public float HorizontalSpeed = 3.0f;
    public float VerticalSpeed = 3.0f;
    public float GravityScale = 4;
    public bool UseGravity;

    [Header("Collisions")]  // Stops us walking into walls
    public float CollisionCheckDistance = 1;
    public Vector2 CollisionCheckOffset = new Vector2(0, 0.5f);
    public LayerMask CollisionCheckLayers;

    [Header("Controllers")]
    public Animator MainAnimator;
    public Rigidbody2D MainRigidbody;
    public Controller DefaultCotroller; // what takes control on 'Start'
    Stack<Controller> Controllers = new Stack<Controller>();

    [HideInInspector] 
    public Vector2 Look;

    void Awake()
    {
        if (DefaultCotroller != null)
        {
            AddController(DefaultCotroller);
        }
    }

    public void AddController (Controller controller)
    {
        if (controller == null) return;

        Debug.Log("Adding Controller");

        void Add ()
        {
            if (Controllers.Count > 0)
                Controllers.Peek().OnControllerPause();
            Controllers.Push(controller);
            controller.OnControllerEnable(this, MainRigidbody, MainAnimator);
        }

        // Give control straight away if first
        if (Controllers.Count == 0)
        {
            Add();
            return;
        }

        // If already in stack, remove control from controllers on top
        if (Controllers.Contains(controller))
        {
            while (Controllers.Peek() != controller)
            {
                Controllers.Pop().OnControllerDisable();
            }
            Add();
        }
        else
        {
            Add();
        }
    }

    public void RemoveController (Controller controller)
    {
        if (controller == null)
        {
            Debug.LogError("Controller is null");
            return;
        }

        if (Controllers.Count == 0)
        {
            Debug.LogWarning("No controllers in stack");
            return;
        }

        if (!Controllers.Contains(controller))
        {
            Debug.LogWarning("Controller doesn't exist in stack");
            return;
        }

        do
        {
            Controllers.Peek().OnControllerDisable();
        }
        while (Controllers.Pop() != controller);

        if (Controllers.Count > 0)
            Debug.Log("New controller: " + Controllers.Peek().name);
    }

    void Update()
    {
        if (Controllers.Count > 0)
        {
                Controllers.Peek().ControllerUpdate();
        }

        if (EventSystem.current.currentSelectedGameObject == null)
        {
            UISceneManager.ClearStack();
            UISceneManager.GiveControl(PlayerReference.Player);
        }
    }

    void FixedUpdate()
    {
        if (Controllers.Count > 0) Controllers.Peek().ControllerFixedUpdate();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        if (Controllers.Count > 0) Controllers.Peek().OnSubmit();
    }

    public void SetGravity (bool value)
    {
        MainRigidbody.gravityScale = value ? GravityScale : 0;
        UseGravity = value;
    }
}
