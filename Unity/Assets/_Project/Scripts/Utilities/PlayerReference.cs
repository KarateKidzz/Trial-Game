using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReference : PlayerDisplay
{
    public static PlayerReference Player { get; private set; }

    [HideInInspector]
    public Actor Actor;

    void Awake()
    {
        Player = this;
        Actor = GetComponent<Actor>();
        if (!Actor)
            Debug.LogError("No Movement!");

        UISceneManager.ClearStack();
        UISceneManager.GiveControl(this);
    }

    void OnDestroy()
    {
        UISceneManager.ClearStack();
    }

    public void MovePlayer(Vector2 teleportPosition, bool useGravityAtNewPos = false, bool setFogOfWar = false)
    {
        Vector3 currentPos = transform.position;
        Vector3 newPos = new Vector3(teleportPosition.x, teleportPosition.y, currentPos.z);
        transform.position = newPos;


        Actor.SetGravity(useGravityAtNewPos);
        FogOfWar.SetFogOfWar(setFogOfWar);

        UISceneManager.GiveControl(this);
    }
}
