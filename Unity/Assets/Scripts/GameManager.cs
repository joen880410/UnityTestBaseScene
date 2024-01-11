using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameStatus
    {
        None,
        Wait,
        Start,
        End,
    }

    public static GameManager instance;
    public List<MapObject> instantiateMaps = new List<MapObject>();
    public List<Player> players = new List<Player>();
    public long playerMoney;
    public int rewardMoney;
    public Player nowPlayPlayer;
    public int index;
    public GameStatus gameStatus = GameStatus.None;
    public const int UpdateTimeValue = 1;
    public DateTime UpdateTime;
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
        gameStatus = GameStatus.Start;
        var index = UnityEngine.Random.Range(0, players.Count);
        ChangePlayer(index);
    }

    // Update is called once per frame
    void Update()
    {
        if (true)
        {
            UpdateTime = DateTime.Now.AddSeconds(UpdateTimeValue);
            //Debug.LogError(UpdateTime);
            switch (gameStatus)
            {
                case GameStatus.None:
                case GameStatus.Wait:
                case GameStatus.End:
                    break;
                case GameStatus.Start:
                    TryCheckPlayerLose();
                    break;
            }
        }
    }

    public void PlayerMove(Player player)
    {
        Debug.Log($"玩家{player.Id}移動:{player.moveStep}步");
        var map = GetMap(player.nowStep % instantiateMaps.Count);
        if (map == null)
        {
            return;
        }
        if (map.owneruid != player.Id && map.owneruid != 0)
        {
            var mapOwner = players.FirstOrDefault(e => e.Id == map.owneruid);
            //3倍過路費給已購買的玩家
            var tolls = map.price * 1;
            mapOwner.money += tolls;
            player.money -= tolls;
            Debug.Log($"玩家{player.Id}給支付過路費:{tolls}給{mapOwner.Id}");
        }
        player.ChangeStat(PlayerStat.Buy);
    }

    public void BuyMap(Player player, bool isBuy)
    {
        var map = GetMap(player.nowStep % instantiateMaps.Count);
        if (map == null)
        {
            return;
        }
        if (map.isCanBuy && map.owneruid == 0)
        {
            if (map.price > player.money)
            {
                Debug.Log($"玩家{player.Id}購買:{map.name}金額不足");
            }
            if (isBuy || player.isAuto)
            {
                Debug.Log($"玩家{player.Id}購買:{map.name}");
                player.money -= map.price;
                map.BuyMap(player.Id);
            }
        }
        ChangeNext();
    }
    /// <summary>
    /// 檢查地圖是否可以購買
    /// </summary>
    /// <returns></returns>
    public bool CanBuy(Player player)
    {
        var map = GetMap(player.nowStep % instantiateMaps.Count);
        if (map == null)
            return false;
        if (map.isCanBuy && map.owneruid == 0)
            return true;
        else
            return false;
    }
    public void TryCheckPlayerLose()
    {
        var losePlayer = players.FirstOrDefault(e => e.money < 0);
        if (losePlayer != null)
        {
            Debug.Log($"玩家{losePlayer.Id}輸了");
            gameStatus = GameStatus.End;
        }
    }

    public MapObject GetMap(int index)
    {
        if (instantiateMaps.Count <= 0)
        {
            return null;
        }
        return instantiateMaps[index];
    }

    public void ChangeNext()
    {
        index = (index + 1) % players.Count;
        ChangePlayer(index);
    }
    private void ChangePlayer(int index)
    {
        if (nowPlayPlayer != null)
        {
            nowPlayPlayer.ChangeStat(PlayerStat.Wait);
        }
        nowPlayPlayer = players[index];
        nowPlayPlayer.ChangeStat(PlayerStat.Move);
        Debug.Log($"玩家{nowPlayPlayer.Id}開始");
    }
}
