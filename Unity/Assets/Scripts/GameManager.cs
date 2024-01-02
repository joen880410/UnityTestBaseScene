using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<MapTiles> maps = new List<MapTiles>();
    public List<Player> players = new List<Player>();
    public long playerMoney;
    public int rewardMoney;
    public int playerCount;
    public Player nowPlayPlayer;
    public int index;
    public bool endGame = false;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //檢查玩家人數
        if (players.Count < playerCount)
        {
            Debug.Log("人數不足");
        }
        //檢查所有玩家的金額
        for (int i = 0; i < players.Count; i++)
        {
            var player = players[i];
            if (player.money < playerMoney)
            {
                player.money = playerMoney;
            }
        }
        Debug.Log("遊戲開始");
        var index = Random.Range(0, playerCount);
        ChangePlayer(index);

        endGame = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (endGame)
            return;
        if (nowPlayPlayer.moveStep != 0)
        {
            PlayerMove();
        }
        PlayerLose();
    }

    private void PlayerMove()
    {
        nowPlayPlayer.nowStep = nowPlayPlayer.moveStep;
        Debug.Log($"玩家{nowPlayPlayer.Id}移動:{nowPlayPlayer.moveStep}步");
        var map = GetMap(nowPlayPlayer.nowStep);
        if (map.owneruid != nowPlayPlayer.Id && map.owneruid != 0)
        {
            var mapOwner = players.FirstOrDefault(e => e.Id == map.owneruid);
            //3倍過路費給已購買的玩家
            var tolls = map.price * 1;
            mapOwner.money += tolls;
            nowPlayPlayer.money -= tolls;
            Debug.Log($"玩家{nowPlayPlayer.Id}給支付過路費:{tolls}給{mapOwner.Id}");
        }
        nowPlayPlayer.ChangeStat(PlayerStat.Buy);
    }

    public void BuyMap(bool isBuy)
    {
        var map = GetMap(nowPlayPlayer.nowStep % maps.Count);
        if (map.isCanBuy && map.owneruid == 0)
        {
            if (map.price > nowPlayPlayer.money)
            {
                Debug.Log($"玩家{nowPlayPlayer.Id}購買:{map.name}金額不足");
            }
            if (isBuy || nowPlayPlayer.isAuto)
            {
                Debug.Log($"玩家{nowPlayPlayer.Id}購買:{map.name}");
                nowPlayPlayer.money -= map.price;
                map.BuyMap(nowPlayPlayer.Id);
            }
        }
        ChangeNext();
    }

    public void PlayerLose()
    {
        var losePlayer = players.FirstOrDefault(e => e.money < 0);
        if (losePlayer != null)
        {
            Debug.Log($"玩家{losePlayer.Id}輸了");
            endGame = true;
        }
    }

    public MapTiles GetMap(int index)
    {
        return maps[index];
    }

    private void ChangeNext()
    {
        index = (index + 1) % playerCount;
        ChangePlayer(index);
    }
    private void ChangePlayer(int index)
    {
        nowPlayPlayer = players[index];
        nowPlayPlayer.ChangeStat(PlayerStat.Move);
        Debug.Log($"玩家{nowPlayPlayer.Id}開始");
    }
}
