using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchConeRotate : MonoBehaviour
{
    [Range(0, 10)]
    public float speed = 2f;
    [Range(1, 360)]
    public float maxRotation = 45f;

    float startRotation;

    void Start()
    {
        startRotation = transform.rotation.eulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        float z = maxRotation * Mathf.Sin(Time.time * speed);
        transform.rotation = Quaternion.Euler(0, 0, z + startRotation);
    }
}
