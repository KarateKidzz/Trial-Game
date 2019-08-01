using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Reference to RectTransform and Rigidbody2D
/// </summary>
public class Projectile
{
    public RectTransform Transform;
    public Rigidbody2D Rigidbody;

    public bool IsNull ()
    {
        return Transform == null || Rigidbody == null;
    }
}

public class EncounterWaveBase : MonoBehaviour
{
    public virtual void InitWave(GameObject _canvasRoot, RectTransform _arenaRect, GameObject _playerObject, float wavetime, int numberOfRounds) { }

    public virtual void UpdateWave() { }

    public virtual void EndWave() { }
}
