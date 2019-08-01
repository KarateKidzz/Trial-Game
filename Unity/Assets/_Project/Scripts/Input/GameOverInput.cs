using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverInput : MonoBehaviour
{

    public int level2Index = 3;
    public int level3Index = 4;

    void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            int overworldScene = SceneHelper.GetOverworldIndex();
            if (overworldScene != -1)
            {
                if (overworldScene == level2Index)
                    SceneHelper.LoadOverworldScene();
                else
                    SceneHelper.LoadNewSceneWithRefresh(level3Index);
            }

        }
    }
}
