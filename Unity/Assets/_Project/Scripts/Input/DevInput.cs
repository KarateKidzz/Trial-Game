using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevInput : MonoBehaviour
{
    bool active;

    void Start()
    {
        transform.GetChild(0).gameObject.SetActive(false);       
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tilde) || Input.GetKeyDown(KeyCode.BackQuote))
        {
            Debug.Log("Pressed");
            active = !active;
            transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.activeInHierarchy);

            Cursor.lockState = active ? CursorLockMode.None : CursorLockMode.Confined;
            Cursor.visible = active;
        }
    }

}
