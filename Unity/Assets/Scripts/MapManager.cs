using System.Collections;
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
            //預設初始座標
            mapObject.transform.position = StartTransform.position;
            //一排有8個地圖
            if (i % 8 != 0)
            {
                var newXPos = mapObject.transform.position.x + i * mapSize;
                mapObject.transform.position = new Vector3(newXPos, mapObject.transform.position.y, mapObject.transform.position.z);
            }
            else if (i % 8 == 0)
            {
                var newXPos = mapObject.transform.position.x + cornerMapSize;
                mapObject.transform.position = new Vector3(newXPos, mapObject.transform.position.y, mapObject.transform.position.z);
            }

            var mapTilesComponent = mapObject.GetComponent<MapObject>();
            mapTilesComponent.SetData(map);
            instantiateMaps.Add(mapTilesComponent);
        }
    }
    public MapObject GetMapObject(int index)
    {
        return instantiateMaps[index];
    }
}
