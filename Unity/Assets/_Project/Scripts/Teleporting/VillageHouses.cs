using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageHouses : MonoBehaviour
{
    public Transform ll, lr, rl, rr;
    public Transform oLL, oLR, oRL, oRR;

    public void MoveToHouseLeftLeft ()
    {
        PlayerReference.Player.MovePlayer(ll.position);
    }

    public void MoveToHouseLeftRight ()
    {
        PlayerReference.Player.MovePlayer(lr.position);
    }

    public void MoveToHouseRightLeft()
    {
        PlayerReference.Player.MovePlayer(rl.position);
    }

    public void MoveToHouseRightRight ()
    {
        PlayerReference.Player.MovePlayer(rr.position);
    }

    public void MoveOutsideLeftLeft ()
    {
        PlayerReference.Player.MovePlayer(oLL.position);
    }

    public void MoveOutsideLeftRight()
    {
        PlayerReference.Player.MovePlayer(oLR.position);
    }

    public void MoveOutsideRightLeft()
    {
        PlayerReference.Player.MovePlayer(oRL.position);
    }

    public void MoveOutsideRightRight()
    {
        PlayerReference.Player.MovePlayer(oRR.position);
    }
}
