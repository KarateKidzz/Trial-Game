using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DelayedProjectile : Projectile
{
    public bool isActive = false;
    public float timeToActivate;
    public float elapsedTime = 0;

    public void Tick (float deltaTime)
    {
        if (isActive) return;

        elapsedTime += deltaTime;

        isActive |= elapsedTime >= timeToActivate;
    }
}

public class BasicProjectileFollow : EncounterWaveBase
{
    public GameObject Prefab;
    readonly List<DelayedProjectile> trackedGameObjects = new List<DelayedProjectile>();

    public float moveSpeed = 10;

    public float projectileSpawnInterval = 0.6f;

    public float minActivateTime = 0.5f;
    public float maxActivateTime = 1.2f;

    float elapsedTime = 0;
    int numRounds;
    int projectilesSpawned;

    GameObject canvas;
    RectTransform arenaRect;
    Vector3[] arenaCorners = new Vector3[4];
    GameObject player;

    void SpawnProjectile ()
    {
        var instantiated = Instantiate(Prefab, arenaCorners[UnityEngine.Random.Range(0, 4)].ReplaceZ(0) * 1.1f, Quaternion.identity, canvas.transform);

        trackedGameObjects.Add(new DelayedProjectile { Transform = instantiated.GetComponent<RectTransform>(), Rigidbody = instantiated.GetComponent<Rigidbody2D>(), timeToActivate = UnityEngine.Random.Range(minActivateTime, maxActivateTime) });

        projectilesSpawned++;
    }

    public override void InitWave(GameObject _canvasRoot, RectTransform _arenaRect, GameObject _playerObject, float wavetime, int numberOfRounds)
    {
        trackedGameObjects.Clear();

        canvas = _canvasRoot;
        arenaRect = _arenaRect;
        arenaRect.GetWorldCorners(arenaCorners);
        player = _playerObject;
        numRounds = numberOfRounds;
        projectilesSpawned = 0;

        SpawnProjectile();
    }

    public override void UpdateWave()
    {
        elapsedTime += Time.deltaTime;

        // Round 0 + 1 = 1, projectiles spawned = 1 ----- Don't spawn
        // Round 1 + 1 = 2, projectiles spawned = 1 ----- Spawn one more
        // Round 1 + 1 = 2, projectiles spawned = 2 ----- No more
        if (numRounds + 1 != projectilesSpawned || numRounds > projectilesSpawned)
        {
            if (elapsedTime >= projectileSpawnInterval)
            {
                elapsedTime = 0;

                SpawnProjectile();
            }
        }

        foreach (DelayedProjectile p in trackedGameObjects)
        {
            if (p == null) continue;
            if (p.IsNull()) continue;

            p.Tick(Time.deltaTime);

            if (!p.isActive) continue;

            // Move

            Vector2 dir = (player.transform.position - p.Transform.position);

            Vector2 vel = dir.normalized * moveSpeed;

            p.Rigidbody.velocity = vel;

            // Look at

            p.Transform.LookAt(player.transform, Vector3.up);
        }
    }

    public override void EndWave ()
    {
        for (int i = 0; i < trackedGameObjects.Count; i++)
        {
            if (trackedGameObjects[i] != null && !trackedGameObjects[i].IsNull())
                Destroy(trackedGameObjects[i].Transform.gameObject);
            else
            {
                trackedGameObjects.RemoveAt(i);
                i--;
            }
        }
        trackedGameObjects.Clear();
    }
}
