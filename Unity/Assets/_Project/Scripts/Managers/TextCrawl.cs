using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;
using System.Linq;
using FMODUnity;

/// <summary>
/// Defines positions in a string where the coroutine should wait a set amount of time
/// </summary>
class IndexWait
{
    public int index = 0;
    public int length = 0;
    public float time = 0;
}

/// <summary>
/// Defines the speed of the text crawl
/// </summary>
public enum CrawlSpeed
{
    OverworldDialogue,
    OverworldInfo,
    Encounter
}

/// <summary>
/// Defines all needed information for a text crawl with shorthands for common settings
/// </summary>
public class CrawlInformation
{
    public TextMeshProUGUI Display;
    public List<string> Texts;
    public CrawlAction OnFinishAction;
    public CrawlSpeed Speed;
    public bool UseRealSeconds;
    public bool PlayerChoosesNextText;
    public bool AllowSkip;
    public float AutoTextFinishTime;
    public string Sound;

    public CrawlInformation (TextMeshProUGUI display, List<string> texts, CrawlAction onFinish, CrawlSpeed speed, bool useRealSeconds, bool waitForPlayer, float defaultWaitTime, bool allowSkip)
    {
        Display = display;
        Texts = texts;
        OnFinishAction = onFinish;
        Speed = speed;
        UseRealSeconds = useRealSeconds;
        PlayerChoosesNextText = waitForPlayer;
        AutoTextFinishTime = defaultWaitTime;
        AllowSkip = allowSkip;
    }

    public CrawlInformation (TextMeshProUGUI display, List<string> texts, CrawlAction onFinish, CrawlSpeed speed, bool useRealSeconds, bool waitForPlayer, float defaultWaitTime, bool allowSkip, string sound) : this(display, texts, onFinish, speed, useRealSeconds, waitForPlayer, defaultWaitTime, allowSkip)
    {
        Sound = sound;
    }

    public static CrawlInformation DefaultEncounter (TextMeshProUGUI targetDisplay, List<string> texts, string soundEvent = "event:/MonsterTextHit")
    {
        return new CrawlInformation(targetDisplay, texts, CrawlAction.Null, CrawlSpeed.Encounter, false, true, 0, true, soundEvent);
    }

    public static CrawlInformation EncounterOnFinish(TextMeshProUGUI targetDisplay, List<string> texts, CrawlAction onFinish, string soundEvent = "event:/MonsterTextHit")
    {
        return new CrawlInformation(targetDisplay, texts, onFinish, CrawlSpeed.Encounter, false, true, 0, true, soundEvent);
    }

    public static CrawlInformation DefaultOverworld(TextMeshProUGUI targetDisplay, List<string> texts)
    {
        return new CrawlInformation(targetDisplay, texts, CrawlAction.Null, CrawlSpeed.OverworldInfo, true, true, 0, true, "event:/TextHit");
    }

    public static CrawlInformation DefaultOverworldDialogue(TextMeshProUGUI targetDisplay, List<string> texts)
    {
        return new CrawlInformation(targetDisplay, texts, CrawlAction.Null, CrawlSpeed.OverworldDialogue, true, true, 0, true, "event:/TextHit");
    }

    public static CrawlInformation OverworldDialogueOnFinish(TextMeshProUGUI targetDisplay, List<string> texts, CrawlAction onFinish)
    {
        return new CrawlInformation(targetDisplay, texts, onFinish, CrawlSpeed.OverworldDialogue, true, true, 0, true, "event:/TextHit");
    }

    public IEnumerator OnFinish ()
    {
        if (OnFinishAction.Action == null)
        {
            yield break;
        }
        if (UseRealSeconds)
            yield return new WaitForSecondsRealtime(OnFinishAction.Delay);
        else
            yield return new WaitForSeconds(OnFinishAction.Delay);

        OnFinishAction.Action.Invoke();
        yield return null;
    }

    public IEnumerator WaitAuto ()
    {
        if (UseRealSeconds)
            yield return new WaitForSecondsRealtime(AutoTextFinishTime);
        else
            yield return new WaitForSeconds(AutoTextFinishTime);
        yield return null;
    }
}

public struct CrawlAction
{
    public Action Action;
    public float Delay;

    public CrawlAction (Action action, float delay)
    {
        Action = action;
        Delay = delay;
    }

    /// <summary>
    /// Shorthand for a <see cref="CrawlAction"/> that does not have an action to invoke
    /// </summary>
    /// <value>The null.</value>
    public static CrawlAction Null
    {
        get => new CrawlAction(null, 0);
    }
}

[CreateAssetMenu]
public class TextCrawl : ScriptableObject
{
    public ScriptableFloat EncounterCrawlSpeed;
    public ScriptableFloat OverworldDialogueSpeed;
    public ScriptableFloat OverworldInfoSpeed;

    static TextCrawl instance; 

    [RuntimeInitializeOnLoadMethod]
    public static void Init()
    {
        instance = Resources.LoadAll<TextCrawl>("")[0];
        instance.EncounterCrawlSpeed.RuntimeValue = instance.EncounterCrawlSpeed.SerializedValue;
        instance.OverworldDialogueSpeed.RuntimeValue = instance.OverworldDialogueSpeed.SerializedValue;
        instance.OverworldInfoSpeed.RuntimeValue = instance.OverworldInfoSpeed.SerializedValue;
        Debug.Log("[Text Crawl] Initialised");
    }

    static Dictionary<TextMeshProUGUI, Coroutine> RunningCrawls = new Dictionary<TextMeshProUGUI, Coroutine>();

    public static Coroutine Crawl(CrawlInformation information)
    {
        Coroutine current;
        Coroutine toRun = CoroutineManager.StartRoutine(instance.CrawlListRoutine(information));
        if (RunningCrawls.TryGetValue(information.Display, out current))
        {
            CoroutineManager.StopRoutine(current);
            RunningCrawls[information.Display] = toRun;
        }
        else
        {
            RunningCrawls.Add(information.Display, toRun);
        }
        return toRun;
    }

    public static void CancelCrawl (TextMeshProUGUI display)
    {
        Coroutine current;
        if (RunningCrawls.TryGetValue(display, out current))
        {
            CoroutineManager.StopRoutine(current);
            display.text = "";
        }
    }

    /// <summary>
    /// Used to diplay a <see cref="CrawlInformation"/>
    /// </summary>
    /// <returns>The list routine.</returns>
    /// <param name="information">Information.</param>
    IEnumerator CrawlListRoutine (CrawlInformation information)
    {
        foreach (string s in information.Texts)
        {
            yield return CrawlRoutine(information, s);
            if (information.PlayerChoosesNextText)
                while (!Input.GetButtonDown("Submit")) yield return null;
            else
                yield return information.WaitAuto();
                
        }
        yield return information.OnFinish();
        yield return null;
    }

    /// <summary>
    /// Used for displaying one string at a time, not using <see cref="CrawlInformation.Texts"/> list
    /// </summary>
    /// <returns>The routine.</returns>
    /// <param name="information">Information.</param>
    /// <param name="text">Text.</param>
    /// <param name="doOnFinish">If set to <c>true</c> do on finish.</param>
    private IEnumerator CrawlRoutine (CrawlInformation information, string text)
    {
        text = "* " + text;
        float waitTime = 0;
        switch (information.Speed)
        {
            case CrawlSpeed.Encounter:
                waitTime = EncounterCrawlSpeed.RuntimeValue;
                break;

            case CrawlSpeed.OverworldDialogue:
                waitTime = OverworldDialogueSpeed.RuntimeValue;
                break;

            case CrawlSpeed.OverworldInfo:
                waitTime = OverworldInfoSpeed.RuntimeValue;
                break;
        }
        List<IndexWait> waits = new List<IndexWait>();

        for (int i = 0; i < text.Length; i++)
        {
            // [W:1.2]
            if (text[i] == '[' && text[i + 1] == 'W' && text[i + 2] == ':')
            {
                int endIndex = i;
                while(text[endIndex] != ']' && endIndex < text.Length)
                {
                    endIndex++;
                }
                float time = 0;
                string substring = text.Substring(i + 3, endIndex - i - 3);
                if (float.TryParse(substring, out time))
                {
                    waits.Add(new IndexWait { index = i, length = endIndex - i + 1, time = time });
                }
                else
                {
                    Debug.LogWarning("[Text Crawl] Failed to parse wait information");
                }
            }
        }
        Dictionary<int, IndexWait> cacheWaits = null;

        if (waits.Count != 0)
        {
            cacheWaits = new Dictionary<int, IndexWait>();

            int charactersRemoved = 0;
            foreach (IndexWait iw in waits)
            {
                iw.index -= charactersRemoved;
                charactersRemoved += iw.length;
                text = text.Remove(iw.index, iw.length);
                cacheWaits.Add(iw.index, iw);
            }
        }

        for (int c = 0; c < text.Length + 1; c++)
        {
            if (information.AllowSkip)
            {
                // Skip on X
                if (Input.GetButton("Cancel"))
                {
                    c = text.Length;
                }
            }

            information.Display.text = text.Substring(0, c);
            if (c < text.Length && text[c] != ' ')
            {
                if (String.IsNullOrEmpty(information.Sound))
                    RuntimeManager.PlayOneShot("event:/TextHit");
                else
                    RuntimeManager.PlayOneShot(information.Sound);
            }
                
            if (cacheWaits != null)
            {
                IndexWait indexWait = null;
                if (cacheWaits.TryGetValue(c, out indexWait))
                {
                    if (information.UseRealSeconds)
                    {
                        yield return new WaitForSecondsRealtime(indexWait.time);
                    }
                    else
                    {
                        yield return new WaitForSeconds(indexWait.time);
                    }
                }
                else
                {
                    if (information.UseRealSeconds)
                    {
                        yield return new WaitForSecondsRealtime(waitTime);
                    }
                    else
                    {
                        yield return new WaitForSeconds(waitTime);
                    }
                }
            }
            else
            {
                if (information.UseRealSeconds)
                {
                    yield return new WaitForSecondsRealtime(waitTime);
                }
                else
                {
                    yield return new WaitForSeconds(waitTime);
                }
            }
        }
        yield return null;
    }
}
