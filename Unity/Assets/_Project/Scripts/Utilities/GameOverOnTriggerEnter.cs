using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverOnTriggerEnter : MonoBehaviour
{
    public LayerMask TriggerLayerMask;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)Mathf.Log(TriggerLayerMask.value, 2))
        {
            GetComponent<Fight>().StartFight();
        }
    }
}
