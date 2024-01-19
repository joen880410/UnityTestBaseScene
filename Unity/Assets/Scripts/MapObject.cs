using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MapTiles;

public class MapObject : MonoBehaviour
{
    public int price;
    public int mapType;
    public Sprite mapImg;
    public bool isCanBuy;
    public bool isCanUpgrade;
    public GameObject mapPlaneObject;

    [Header("轉動玩家角度，控制玩家前進方向")]
    public Vector3 playerRotationAngle;

    [Header("地圖大小")]
    public Vector3 mapSize;

    public long owneruid;
    public MapStage stage;
    public void SetData(MapTiles mapTiles)
    {
        price = mapTiles.price;
        owneruid = mapTiles.owneruid;
        mapType = mapTiles.mapType;
        isCanBuy = mapTiles.isCanBuy;
        stage = mapTiles.stage;
        mapImg = mapTiles.mapImg;
        mapPlaneObject.transform.localScale = mapTiles.mapSize;
    }
    public void BuyMap(int playerUid)
    {
        owneruid = playerUid;
        stage = MapStage.stage1;
    }
}
