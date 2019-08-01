using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class EncounterCollisionEvent : UnityEvent<Collider2D, EncounterAttackerBase> { }

public class HeartController : MonoBehaviour
{
    Rigidbody2D rb;

    float horizontal, vertical;

    public float horizontalSpeed = 2;
    public float verticalSpeed = 2;

    public bool PlayerHasControl = true;

    public EncounterCollisionEvent OnHitMethod;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Debug.Assert(rb);
    }

    void Update()
    {
        if (PlayerHasControl)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
        }
        else
        {
            horizontal = 0;
            vertical = 0;
        }
    }

    void FixedUpdate()
    {
        if (PlayerHasControl)
        {
            Vector2 position = rb.position;
            position.x = position.x + horizontalSpeed * horizontal * Time.deltaTime;
            position.y = position.y + verticalSpeed * vertical * Time.deltaTime;

            rb.MovePosition(position);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        OnHitMethod.Invoke(collision, collision.GetComponent<EncounterAttackerBase>());
    }
}
