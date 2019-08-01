using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuNavigation : MonoBehaviour
{
    public EventSystem EventSystem;
    public GameObject StartButton;

    void OnEnable ()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        EventSystem.SetSelectedGameObject(null);
        StartCoroutine(SetCurrentWithDelay());
    }

    IEnumerator SetCurrentWithDelay ()
    {
        yield return null;
        EventSystem.SetSelectedGameObject(StartButton);
        yield return null;
    }

    void Update()
    {
        if (EventSystem.currentSelectedGameObject == null)
        {
            
            if (InputHelp.AnyArrorKey())
            {
                Debug.Log("Setting object");
                EventSystem.SetSelectedGameObject(StartButton);
            }
        }
    }
}
