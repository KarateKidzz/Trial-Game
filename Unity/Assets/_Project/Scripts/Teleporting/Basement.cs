using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basement : MonoBehaviour
{
    public Transform BasementDoor, HallwayDoor, BasementTwoDoor, HallwayTwoDoor;

    public void MoveToBasement ()
    {
        PlayerReference.Player.MovePlayer(BasementDoor.position);
    }

    public void MoveToHallway()
    {
        PlayerReference.Player.MovePlayer(HallwayDoor.position);
    }

    public void MoveToSecondHallway()
    {
        PlayerReference.Player.MovePlayer(HallwayTwoDoor.position);
    }

    public void MoveToSecondDungeon()
    {
        PlayerReference.Player.MovePlayer(BasementTwoDoor.position);
    }
}
