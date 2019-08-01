using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComingSoonInput : MonoBehaviour
{
    void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            Debug.Log("Submit");
            SceneHelper.LoadNewScene(0);
        }
    }
}
