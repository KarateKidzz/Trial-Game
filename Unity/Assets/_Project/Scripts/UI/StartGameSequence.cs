using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using FMODUnity;

public class StartGameSequence : MonoBehaviour
{
    public StudioEventEmitter Emitter;
    public int StartSceneIndex = 1;
    public Image WhiteBackground;
    public TextMeshProUGUI Text;

    public GameObject Title;
    public GameObject Buttons;
    public GameObject Rain;

    public string StartText;
    public int FadeSteps = 100;
    public float FadeInTime = 1;
    public float FadeOutTime = 1;
    public float TimeBetweenCharacters = 0.2f;
    public float TimeBetweenBackgroundAndText = 0.5f;
    public float TimeBeforeFadeOut = 2;

    public void StartGame ()
    {
        // Make sure no text is displayed
        Text.text = "";

        StartCoroutine(Sequence());
    }

    IEnumerator Sequence ()
    {
        PlayerInventory.GetItems().Clear();

        AsyncOperation loadingScene = SceneManager.LoadSceneAsync(StartSceneIndex, LoadSceneMode.Additive);
        loadingScene.allowSceneActivation = false;

        Cursor.visible = false;

        WhiteBackground.gameObject.SetActive(true);
        Buttons.SetActive(false);
        Rain.SetActive(false);

        Emitter.Stop();

        for (float i = 0; i <= FadeInTime; i += Time.deltaTime)
        {
            WhiteBackground.color = new Color(1, 1, 1, i / FadeInTime);

            yield return null;
        }

        Title.SetActive(false);

        yield return new WaitForSeconds(TimeBetweenBackgroundAndText);

        // iterate characters in string
        for (int c = 0; c < StartText.Length + 1; c++)
        {
            Text.SetText(StartText.Substring(0, c));
            if (c < StartText.Length && StartText[c] != ' ')
                RuntimeManager.PlayOneShot("event:/TextHit");
            yield return new WaitForSeconds(TimeBetweenCharacters);
        }

        yield return new WaitForSeconds(TimeBeforeFadeOut);

        for (float i = FadeOutTime; i >= 0; i -= Time.deltaTime)
        {
            WhiteBackground.color = new Color(1, 1, 1, i / FadeOutTime);

            yield return null;
        }

        loadingScene.allowSceneActivation = true;

        while (!loadingScene.isDone)
        {
            yield return null;
        }


        var loadedScene = SceneManager.GetSceneAt(1);
        SceneManager.SetActiveScene(loadedScene);
        SceneManager.UnloadSceneAsync(gameObject.scene);

        Time.timeScale = 1;

        yield return null;
    }

}
