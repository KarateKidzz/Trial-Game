using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingProjectile : EncounterWaveBase
{
    public GameObject Prefab;
    readonly List<GameObject> trackedGameObjects = new List<GameObject>();

    [Range(0, 5)]
    public float MinFallSpeed = 2;
    [Range(2, 10)]
    public float MaxFallSpeed = 4;

    public float projectileSpawnInterval = 0.4f;

    Vector2 topLeft, topRight;
    GameObject canvas;
    float elapsedTime;
    int numRounds;
    int projectilesSpawned;

    void SpawnProjectile()
    {
        var instantiated = Instantiate(Prefab, new Vector3(Mathf.Lerp(topLeft.x, topRight.x, Random.Range(0.0f, 1.0f)), topLeft.y + 2, 0), Quaternion.identity, canvas.transform);
        Rigidbody2D rb = instantiated.GetComponent<Rigidbody2D>();
        if (!rb) Debug.LogError("No Rigidbody on Prefab");

        rb.velocity = new Vector2(0, -Random.Range(MinFallSpeed, MaxFallSpeed));


        trackedGameObjects.Add(instantiated);

        projectilesSpawned++;
    }

    public override void InitWave(GameObject _canvasRoot, RectTransform _arenaRect, GameObject _playerObject, float wavetime, int numberOfRounds)
    {
        trackedGameObjects.Clear();

        numRounds = numberOfRounds;
        canvas = _canvasRoot;

        Vector3[] corners = new Vector3[4];
        _arenaRect.GetWorldCorners(corners);

        topLeft = corners[1];
        topRight = corners[2];

        int numberToSpawn = numberOfRounds + 2;

        SpawnProjectile();

        for (int i = 0; i < numberToSpawn; i++)
        {
            Invoke("SpawnProjectile", Random.Range(0.5f, 3.0f));
        }
    }

    public override void EndWave()
    {
        for (int i = 0; i < trackedGameObjects.Count; i++)
        {
            if (trackedGameObjects[i] != null)
                Destroy(trackedGameObjects[i]);
            else
            {
                trackedGameObjects.RemoveAt(i);
                i--;
            }
        }
        trackedGameObjects.Clear();
    }
}
