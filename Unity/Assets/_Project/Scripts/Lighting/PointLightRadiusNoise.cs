using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class PointLightRadiusNoise : MonoBehaviour
{
    Light Light;

    public float Range;
    public float Speed;

    public float offset = 60;

    void Awake()
    {
        Light = GetComponent<Light>();
    }

    void Start()
    {
        Debug.Assert(Light.type == LightType.Spot);
    }

    void Update()
    {
        float newRadius = Mathf.Sin(Time.time * Speed);

        Light.spotAngle = newRadius * Range + offset;
    }
}
