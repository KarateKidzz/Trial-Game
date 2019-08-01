using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

[CreateAssetMenu]
public class SceneHelper : ScriptableObject
{
    public static SceneHelper Instance { get; private set; }

    [RuntimeInitializeOnLoadMethod]
    public static void Init()
    {
        Instance = Resources.LoadAll<SceneHelper>("")[0];
        Debug.Log("[Scene Helper] Initialised");
    }

    public int MenuScene = 0;
    public int EncounterScene = -1;
    public int GameOverScene;
    /// <summary>
    /// The overworld/gameplay scene that was last active
    /// </summary>
    [ReadOnly]
    public int PreviousActiveScene = -1;

    public void l_LoadBattleScene ()
    {
        LoadBattleScene();
    }

    public void l_LoadNextScene ()
    {
        LoadNextScene();
    }

    /// <summary>
    /// Loads the battle scene and disables the current scene
    /// </summary>
    public static void LoadBattleScene ()
    {
        Debug.Log("[Scene Helper] Loading Battle Scene");
        CoroutineManager.StartRoutine(LoadSceneAndSetActive(Instance.EncounterScene, LoadSceneMode.Additive, true));
    }

    public static void LoadPreviousScene ()
    {
        Debug.Log("[Scene Helper] Loading Previous Scene");
        CoroutineManager.StartRoutine(LoadSceneAndSetActive(Instance.PreviousActiveScene, LoadSceneMode.Additive));
    }

    public static void LoadGameOverScene ()
    {
        Debug.Log("[Scene Helper] Loading GameOver Scene");
        CoroutineManager.StartRoutine(LoadSceneAndSetActive(Instance.GameOverScene, LoadSceneMode.Additive));
    }

    public static void LoadNewScene (int buildIndex)
    {
        CoroutineManager.StartRoutine(LoadSceneAndSetActive(buildIndex, LoadSceneMode.Single));
    }

    public static void LoadNewSceneWithRefresh (int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }



    /// <summary>
    /// ONLY USE IF NOT IN OVERWORLD
    /// </summary>
    public static void LoadOverworldScene ()
    {
        Debug.Log("[Scene Helper] Loading Overworld");
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        Scene[] loadedScenes = GetAllScenes();
        for (int i = 0; i < loadedScenes.Length; i++)
        {
            // If different index, must be the overworld
            if (loadedScenes[i].buildIndex != currentIndex)
            {
                CoroutineManager.StartRoutine(LoadSceneAndSetActive(loadedScenes[i].buildIndex, LoadSceneMode.Additive));
            }
        }
    }

    public static int GetOverworldIndex()
    {
        Debug.Log("[Scene Helper] Loading Overworld");
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        Scene[] loadedScenes = GetAllScenes();
        for (int i = 0; i < loadedScenes.Length; i++)
        {
            // If different index, must be the overworld
            if (loadedScenes[i].buildIndex != currentIndex)
            {
                return loadedScenes[i].buildIndex;
            }
        }
        return -1;
    }

    static Scene[] GetAllScenes ()
    {
        Scene[] array = new Scene[SceneManager.sceneCount];
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = SceneManager.GetSceneAt(i);
        }
        return array;
    }

    public static void LoadNextScene ()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        CoroutineManager.StartRoutine(LoadSceneAndSetActive(currentIndex + 1, LoadSceneMode.Additive));
    }

    static IEnumerator LoadSceneAndSetActive (int sceneIndex, LoadSceneMode loadSceneMode, bool onlyDisable = false)
    {
        SerializedPlayer.Reset();

        Scene activeScene = SceneManager.GetActiveScene();
        Instance.PreviousActiveScene = activeScene.buildIndex;

        if (activeScene.buildIndex == sceneIndex)
        {
            Debug.LogWarning("[Scene Helper] The previous scene is the same as the current. The scene will be reloaded");
            SceneManager.LoadScene(sceneIndex);
            yield break;
        }


        Scene loadScene = SceneManager.GetSceneByBuildIndex(sceneIndex);
        if (!loadScene.isLoaded || loadScene.handle == 0)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex, loadSceneMode);
            asyncOperation.allowSceneActivation = true;
            while (!asyncOperation.isDone) yield return null;
            loadScene = SceneManager.GetSceneByBuildIndex(sceneIndex);
        }
        else
        {
            loadScene.GetRootGameObjects()[0].SetActive(true);
        }
        SceneManager.SetActiveScene(loadScene);

        if (!onlyDisable)
        {
            var unload = SceneManager.UnloadSceneAsync(activeScene);
            while (!unload.isDone) yield return null;
        }
        else
        {
            activeScene.GetRootGameObjects()[0].SetActive(false);
        }


        yield return null;
    }
}
