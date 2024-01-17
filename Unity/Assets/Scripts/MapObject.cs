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

    [Tooltip("轉動玩家角度，控制玩家前進方向")]
    public Vector3 playerRotationAngle;

    public long owneruid;
    public MapStage stage;
    public void BuyMap(int playerUid)
    {
        owneruid = playerUid;
        stage = MapStage.stage1;
    }
}
