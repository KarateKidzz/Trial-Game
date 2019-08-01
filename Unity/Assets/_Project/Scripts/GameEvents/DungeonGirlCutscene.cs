using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DungeonGirlCutscene : MonoBehaviour
{
    public DungeonCameraController CameraController;
    public BlankController PlayerBlankController;
    public GameObject ExitDoorNormal, ExitDoorNextLevel;
    public Transform GirlWalkToDoor;
    public Actor Girl;
    public ActorFollowPathController FollowPathController;
    public List<string> GirlDialogue = new List<string>();

    bool completed;

    public void StartCutscene ()
    {
        if (completed) return;
        // Show girl
        Girl.gameObject.SetActive(true);

        // Disable player
        PlayerReference.Player.Actor.AddController(PlayerBlankController);

        Invoke("DelayStart", 1);
    }

    void DelayStart ()
    {
        // Move the camera
        CameraController.StartCutscene();

        // Dialogue
        Invoke("StartWalk", 2.5f);
    }

    void StartWalk ()
    {
        Girl.AddController(FollowPathController);
        FollowPathController.OnPathFinished += WalkFinished;
    }

    void WalkFinished ()
    {
        TextDisplay.DisplayBasicTextExtra(new CrawlInformation(null, GirlDialogue, new CrawlAction(TalkFinished, 0), CrawlSpeed.OverworldDialogue, false, false, 0.5f, false), false);
    }

    void TalkFinished ()
    {
        TextDisplay.StaticRemoveControl();
        CameraController.EndCutscene();

        Girl.HorizontalSpeed = 2;
        Girl.VerticalSpeed = 2;

        FollowPathController.target = GirlWalkToDoor;
        FollowPathController.FindPath();

        FollowPathController.OnPathFinished -= WalkFinished;
        FollowPathController.OnPathFinished += RemoveGirl;
    }

    void GiveControl ()
    {
        PlayerReference.Player.Actor.RemoveController(PlayerBlankController);
    }

    void RemoveGirl ()
    {
        PlayerReference.Player.Actor.RemoveController(PlayerBlankController);

        ExitDoorNormal.SetActive(false);
        ExitDoorNextLevel.SetActive(true);
        Girl.gameObject.SetActive(false);
        completed = true;
    }
}
