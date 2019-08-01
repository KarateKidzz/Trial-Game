using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// Manages text UI for general gameplay. Examples being signs, dialogue
/// </summary>
public class TextDisplay : PlayerDisplay, ISubmitHandler
{
    public GameObject Parent;
    public GameObject TitleParent;

    public TextMeshProUGUI TitleText;
    public TextMeshProUGUI BodyText;

    static TextDisplay instance;

    bool lockPlayer;

    // ----- PlayerDisplay Overriding ----- //

    public override void Open()
    {
        instance.lockPlayer = false;
        Parent.SetActive(true);
        Time.timeScale = 0;
    }

    public override void Close()
    {
        Parent.SetActive(false);
        Time.timeScale = 1;
        TextCrawl.CancelCrawl(instance.BodyText);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        if (!lockPlayer)
        {
            RemoveControl();
        }
    }

    // ----- PlayerDisplay Overriding ----- //

    void Awake()
    {
        instance = this;
    }

    public static void DisplayBasicText(string text, Action endMessageCall = null, bool crawl = true, bool noPlayerControl = false)
    {
        instance.TakeControl();

        TextCrawl.CancelCrawl(instance.BodyText);

        instance.lockPlayer = noPlayerControl;

        // Remove title
        instance.TitleText.SetText("");
        instance.TitleParent.SetActive(false);

        if (!crawl)
            instance.BodyText.SetText(text);

        // Show object
        instance.Parent.SetActive(true);

        if (crawl)
            TextCrawl.Crawl(CrawlInformation.OverworldDialogueOnFinish(instance.BodyText, new List<string> { text }, new CrawlAction(endMessageCall, 0)));
    }

    public static void DisplayBasicTextExtra(CrawlInformation crawlInformation, bool pauseGame = true)
    {
        instance.TakeControl();

        if (!pauseGame) Time.timeScale = 1;

        instance.lockPlayer = true;
        crawlInformation.Display = instance.BodyText;

        TextCrawl.CancelCrawl(instance.BodyText);

        // Remove title
        instance.TitleText.SetText("");
        instance.TitleParent.SetActive(false);

        // Show object
        instance.Parent.SetActive(true);

        TextCrawl.Crawl(crawlInformation);
    }

    public static void StaticRemoveControl()
    {
        instance.RemoveControl();
    }
}
