using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public List<MapTiles> maps = new List<MapTiles>();
    public async void Awake()
    {
        for (int i = 0; i < maps.Count; i++)
        {
            var map = maps[i];
            var result = (await AddrssableAsync.instance.LoadAsync("Map")).WaitForCompletion();
            if (result == null)
            {
                Debug.LogError($"§ä¤£¨ì¸ê·½:{map.name}");
                continue;
            }
            var mapObject = (GameObject)GameObject.Instantiate(result, this.gameObject.transform);
            mapObject.name = map.name;
            var mapTilesComponent = mapObject.GetComponent<MapObject>();
            mapTilesComponent.price = map.price;
            mapTilesComponent.owneruid = map.owneruid;
            mapTilesComponent.mapType = map.mapType;
            mapTilesComponent.isCanBuy = map.isCanBuy;
            mapTilesComponent.stage = map.stage;
            mapTilesComponent.mapImg = map.mapImg;
            GameManager.instance.instantiateMaps.Add(mapTilesComponent);
        }
    }
}
