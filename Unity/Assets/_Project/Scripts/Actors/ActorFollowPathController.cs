using System;
using UnityEngine;
using Pathfinding;

public class ActorFollowPathController : Controller
{
    public Transform target;
    public Seeker seeker;
    public float nextWaypointDistance = 3;

    public event Action OnPathFinished;

    Path path;
    int currentWaypoint;
    bool reachedEnd;

    protected override void OnControllerEnableHook()
    {
        CancelInvoke();
        Invoke("FindPath", 0.5f);
    }

    public override void DoMovement()
    {
        if (path == null || reachedEnd)
        {
            Move.Set(0, 0);
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEnd = true;
            Debug.Log("Reached end");
            OnPathFinished?.Invoke();
            Move.Set(0, 0);
            //Actor.RemoveController(this);
            return;
        }
      
        Move = ((Vector2)path.vectorPath[currentWaypoint] - MainRigidbody.position).normalized;

        float distance = Vector2.Distance(MainRigidbody.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    void OnPathComplete (Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
            reachedEnd = false;
        }
        else
        {
            Debug.LogWarning(p.errorLog);
        }
    }

    public void FindPath ()
    {
        seeker.StartPath(MainRigidbody.position, target.position, OnPathComplete);
    }
}
