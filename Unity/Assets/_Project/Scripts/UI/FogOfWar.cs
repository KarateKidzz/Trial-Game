using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FogOfWar : MonoBehaviour
{
    public Tilemap darkMap;
    public Tilemap backgroundMap;

    public Tile darkTile;

    public SMask mask;

    static FogOfWar instance;

    void Awake()
    {
        instance = this;
    }

    void OnDestroy()
    {
        instance = null;
    }

    void Start()
    {
        darkMap.origin = backgroundMap.origin;
        darkMap.size = backgroundMap.size;

        foreach (Vector3Int p in darkMap.cellBounds.allPositionsWithin)
        {
            darkMap.SetTile(p, darkTile);
        }

        darkMap.gameObject.SetActive(false);
    }

    public static void SetFogOfWar (bool value)
    {
        if (instance != null)
        {
            instance.mask.transform.position = PlayerReference.Player.transform.position;
            instance.darkMap.gameObject.SetActive(value);
        }
    }
}
