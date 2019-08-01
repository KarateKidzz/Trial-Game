using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryOnRelease : MonoBehaviour
{
#if !UNITY_EDITOR
    void Start()
    {
        Destroy(gameObject);
    }
#endif
}
