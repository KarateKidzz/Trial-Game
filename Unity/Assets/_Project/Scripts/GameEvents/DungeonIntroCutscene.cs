using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class DungeonIntroCutscene : MonoBehaviour
{
    public Image BlackImage;
    public CinemachineVirtualCamera StartCamera;
    public CinemachineVirtualCamera GroupCamera;
    public BlankController PlayerBlankController;
    public GameObject Mole;

    public List<string> MoleDialogue = new List<string>();

    void Start()
    {
        PlayerReference.Player.Actor.AddController(PlayerBlankController);

        StartCamera.Priority = 20;

        StartCoroutine(FadeCanvas());
    }

    IEnumerator FadeCanvas ()
    {
        for (float i = 3; i >= 0; i -= Time.deltaTime)
        {
            BlackImage.color = new Color(0, 0, 0, i / 3);
            yield return null;
        }

        ZoomCamera();

        yield return new WaitForSeconds(2.2f);

        TextDisplay.DisplayBasicTextExtra(new CrawlInformation(null, MoleDialogue, new CrawlAction(() => StartCoroutine(Finish()), 0.5f), CrawlSpeed.OverworldDialogue, false, false, 0.5f, false), false);

        yield return null;
    }

    void ZoomCamera ()
    {
        StartCamera.Priority = 5;
        GroupCamera.Priority = 20;
    }

    IEnumerator Finish ()
    {
        for (float i = 0; i < 3; i += Time.deltaTime)
        {
            BlackImage.color = new Color(0, 0, 0, i / 3);
            yield return null;
        }
        Mole.SetActive(false);
        PlayerReference.Player.Actor.RemoveController(PlayerBlankController);

        StartCamera.Priority = 5;
        GroupCamera.Priority = 5;

        TextDisplay.StaticRemoveControl();

        yield return new WaitForSeconds(2);

        for (float i = 2; i >= 0; i -= Time.deltaTime)
        {
            BlackImage.color = new Color(0, 0, 0, i / 3);
            yield return null;
        }

        yield return null;
    }
}
