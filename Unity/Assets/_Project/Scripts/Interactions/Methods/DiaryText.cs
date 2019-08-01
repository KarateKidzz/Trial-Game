using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaryText : MonoBehaviour
{
    [TextArea]
    public string Diary1Text;

    public void InvokeDiary1 ()
    {
        TextDisplay.DisplayBasicText(Diary1Text);
    }
}
