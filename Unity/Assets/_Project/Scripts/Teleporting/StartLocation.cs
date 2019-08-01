using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLocation : MonoBehaviour
{
    public Transform startLocation;
    
    public void MoveToStart ()
    {
        PlayerReference.Player.MovePlayer(startLocation.position);
    }
}
