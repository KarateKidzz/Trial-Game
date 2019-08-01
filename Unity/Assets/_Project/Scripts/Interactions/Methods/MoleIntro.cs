using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

public class MoleIntro : MonoBehaviour
{
    [TextArea]
    public string dialogue;

    public LayerMask interactLayer;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)Mathf.Log(interactLayer.value, 2))
        {
            //TextCrawl.Crawl(new CrawlInformation(TextDisplay.StaticBodyText, new List<string> { dialogue }, new CrawlAction(OnMoleEnd, 0.5f), CrawlSpeed.OverworldDialogue, true, false, 0.5f));
            //TextDisplay.DisplayBasicText(dialogue, OnMoleEnd, true, true);
            TextDisplay.DisplayBasicTextExtra(new CrawlInformation(null, new List<string> { dialogue }, new CrawlAction(OnMoleEnd, 0), CrawlSpeed.OverworldDialogue, true, false, 0, false));
        }
    }

    void OnMoleEnd ()
    {
        MoleIntroUI.PlayFlash();
    }
}
