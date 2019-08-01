using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AIActorMovement : MonoBehaviour
{
    [Header("Movement Values")]
    public float horizontalSpeed = 3.0f;
    public float verticalSpeed = 3.0f;

    [Header("Init Values")]
    [Range(-1, 1)]
    public float lookHorInit = 0;
    [Range(-1, 1)]
    public float lookVerInit = 0;

    [SerializeField]
    bool useGravity = false;
    public bool UseGravity
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
    readonly Vector2 look;

    [Header("Controls")]
    [Range(-1.0f, 1.0f)]
    public float horizontal;
    [Range(-1.0f, 1.0f)]
    public float vertical;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        animator.SetFloat(hashLookX, lookHorInit);
        animator.SetFloat(hashLookY, lookVerInit);

        UseGravity = useGravity;
    }

    void Update()
    {
        if (UseGravity) vertical = 0;

        var move = new Vector2(horizontal, vertical);

        // Set our direction as long as we're not standing still
        // Otherwise, we wouldn't have a direction
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            look.Set(move.x, move.y);
            look.Normalize();
        }

        animator.SetFloat(hashLookX, look.x);
        animator.SetFloat(hashLookY, look.y);

        animator.SetFloat(hashSpeed, move.magnitude);

        Vector2 position = rb.position;
        position.x = position.x + horizontalSpeed * horizontal * Time.deltaTime;
        position.y = position.y + verticalSpeed * vertical * Time.deltaTime;


        rb.MovePosition(position);
    }
}
