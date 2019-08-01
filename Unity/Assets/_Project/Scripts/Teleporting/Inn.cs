using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inn : MonoBehaviour
{
    public Transform insideLocation;
    public Transform outsideLocation;

    public void MoveToOutside ()
    {
        PlayerReference.Player.MovePlayer(outsideLocation.position);
    }

    public void MoveToInside ()
    {
        PlayerReference.Player.MovePlayer(insideLocation.position);
    }
}
