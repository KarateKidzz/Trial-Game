using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Displays text to the UI without a header/title/name
/// </summary>
public class SignText : MonoBehaviour
{
    [TextArea]
    public string Text;
    public bool Crawl;

    public void Invoke ()
    {
        TextDisplay.DisplayBasicText(Text, null, Crawl);
    }
}
