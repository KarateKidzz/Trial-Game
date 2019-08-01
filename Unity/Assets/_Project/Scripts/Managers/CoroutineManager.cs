using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
    static CoroutineManager instance;
    static CoroutineManager Instance
    {
        get
        {
            if (instance == null)
            {
                var go = new GameObject("Coroutine Manager");

                instance = go.AddComponent<CoroutineManager>();

                DontDestroyOnLoad(go);
            }

            return instance;
        }
    }

    public static Coroutine StartRoutine (IEnumerator routine)
    {
        return Instance.StartCoroutine(routine);
    }

    public static void StopRoutine (Coroutine coroutine)
    {
        instance.StopCoroutine(coroutine);
    }
}
