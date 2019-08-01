 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;

public class DungeonCameraController : MonoBehaviour
{
    public CinemachineVirtualCamera GirlCamera;

    public void StartCutscene ()
    {
        GirlCamera.Priority *= 2;
    }

    public void EndCutscene ()
    {
        GirlCamera.Priority /= 2;
    }

}
