using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoleIntroUI : MonoBehaviour
{
    public Image redFlashImage;
    public Image backgroundImage;
    public float fadeInTime = 0.2f;
    public float fadeOutTime = 3.0f;

    static MoleIntroUI instance;

    void Awake()
    {
        instance = this;
    }

    public static void PlayFlash ()
    {
        UIGroupManager.CloseDisplays();
        instance.StartCoroutine(instance.RedFlashSequence());
        Time.timeScale = 0;
    }

    IEnumerator RedFlashSequence ()
    {
        Color orig = redFlashImage.color;
        for (float i = 0; i <= fadeInTime; i += Time.unscaledDeltaTime)
        {
            redFlashImage.color = new Color(orig.r, orig.g, orig.b, i / fadeInTime);
            yield return null;
        }

        backgroundImage.color = new Color(backgroundImage.color.r, backgroundImage.color.g, backgroundImage.color.b, 1);

        for (float i = fadeOutTime; i >= 0; i -= Time.unscaledDeltaTime)
        {
            redFlashImage.color = new Color(orig.r, orig.g, orig.b, i / fadeOutTime);
            yield return null;
        }
        Time.timeScale = 1;

        SceneHelper.LoadNextScene();

        yield return null;
    }
}
