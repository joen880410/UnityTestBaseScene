﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField]
    private List<MapTiles> maps = new List<MapTiles>();
    private List<MapObject> instantiateMaps = new List<MapObject>();
    public Transform StartTransform;
    public int mapSize;
    public int cornerMapSize;

    public int mapCount
    {
        get
        {
            return instantiateMaps.Count;
        }
    }
    public async void Awake()
    {
        for (int i = 0; i < maps.Count; i++)
        {
            var map = maps[i];
            var result = (await AddrssableAsync.LoadAsync("Map")).WaitForCompletion();
            if (result == null)
            {
                Debug.LogError($"找不到資源:{map.name}");
                continue;
            }
            var mapObject = (GameObject)GameObject.Instantiate(result, this.gameObject.transform);

            mapObject.name = map.name;
            instantiateMaps.Add(mapObject.GetComponent<MapObject>());
        }
    }
    public MapObject GetMapObject(int index)
    {
        return instantiateMaps[index];
    }
}
