using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZAxisMover : MonoBehaviour
{
    public float Speed;
    public float Range;
    public float offset;

    Vector2 xy;

    void Start()
    {
        xy = transform.position;
    }

    void Update()
    {
        float newRadius = Mathf.Sin(Time.time * Speed);

        transform.position = new Vector3(xy.x, xy.y, newRadius * Range + offset);
        //transform.position.Set(transform.position.x, transform.position.y, newRadius * Range + offset);
    }
}
