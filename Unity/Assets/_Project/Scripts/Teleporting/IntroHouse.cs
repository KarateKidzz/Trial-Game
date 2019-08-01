using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroHouse : MonoBehaviour
{
    public Transform InsideHouseLocation;
    public Transform OutsideHouseLocation;
    public Transform BasementLocation;

    public void MoveToInside ()
    {
        PlayerReference.Player.MovePlayer(InsideHouseLocation.position);
    }

    public void MoveToOutside ()
    {
        PlayerReference.Player.MovePlayer(OutsideHouseLocation.position);
    }

    public void MoveToBasement ()
    {
        PlayerReference.Player.MovePlayer(BasementLocation.position, true, true);
    }
}
