using UnityEngine;

[CreateAssetMenu]
public class MapTiles : ScriptableObject
{
    public enum MapStage
    {
        stage1,
        stage2,
        stage3,
        stage4
    }
    public int price;
    public int mapType;
    public Sprite mapImg;
    public bool isCanBuy;
    public bool isCanUpgrade;

    [Tooltip("轉動玩家角度，控制玩家前進方向")]
    public Vector3 playerRotationAngle;

    public long owneruid;
    [Header("地圖目前狀態")]
    public MapStage stage;
    public void BuyMap(int playerUid)
    {
        owneruid = playerUid;
        stage = MapStage.stage1;
    }
}
