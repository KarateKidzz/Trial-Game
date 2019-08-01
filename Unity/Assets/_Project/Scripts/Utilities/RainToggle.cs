using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class RainToggle : MonoBehaviour
{
    ParticleSystem system;

    void Awake()
    {
        system = GetComponent<ParticleSystem>();
    }

    public void TurnOff ()
    {
        system.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    public void TurnOn ()
    {
        if (!system.isPlaying)
            system.Play(true);
    }
}
