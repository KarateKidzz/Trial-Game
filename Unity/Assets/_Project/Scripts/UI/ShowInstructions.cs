using UnityEngine;
using System.Collections;
using FMODUnity;

public class ShowInstructions : MonoBehaviour
{
	public GameObject Menu, Instructions;
    public StudioEventEmitter Emitter;

    void Awake()
    {
        Menu.SetActive(true);
        Instructions.SetActive(false);
        enabled = false;
    }

    public void Show ()
    {
        Menu.SetActive(false);
        Instructions.SetActive(true);
        StartCoroutine(SetActiveWithDelay());
    }

    // When pressing the button to show the controls, this object becomes active and also sees the input. We need to wait a frame before accepting input
    IEnumerator SetActiveWithDelay ()
    {
        yield return null;
        enabled = true;
        yield return null;
    }

    void Update()
    {
        if (InputHelp.Confirm())
        {
            Menu.SetActive(true);
            Instructions.SetActive(false);
            enabled = false;
            Emitter.Play();
        }
    }
}
