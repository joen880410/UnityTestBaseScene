﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<MapTiles> maps = new List<MapTiles>();
    public List<MapTiles> instantiateMaps = new List<MapTiles>();
    public List<Player> players = new List<Player>();
    public long playerMoney;
    public int rewardMoney;
    public Player nowPlayPlayer;
    public int index;
    public bool endGame = false;
    private async void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        GameObject MapGroup = new GameObject("MapGroup");
        for (int i = 0; i < maps.Count; i++)
        {
            var map = maps[i];
            var result = (await AddrssableAsync.instance.LoadAsync("Map")).WaitForCompletion();
            if (result == null)
            {
                Debug.LogError($"找不到資源:{map.name}");
                continue;
            }
            var mapObject = (GameObject)GameObject.Instantiate(result, MapGroup.transform);
            var mapTilesComponent = mapObject.GetComponent<MapObject>();
            mapTilesComponent.mapTiles = map;
            instantiateMaps.Add(mapTilesComponent.mapTiles);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (instantiateMaps.Count != maps.Count)
        {
            return;
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
        var index = Random.Range(0, players.Count);
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
        if (map == null)
        {
            return;
        }
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
        var map = GetMap(nowPlayPlayer.nowStep % instantiateMaps.Count);
        if (map == null)
        {
            return;
        }
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
        if (instantiateMaps.Count <= 0)
        {
            return null;
        }
        return instantiateMaps[index];
    }

    private void ChangeNext()
    {
        index = (index + 1) % players.Count;
        ChangePlayer(index);
    }
    private void ChangePlayer(int index)
    {
        nowPlayPlayer = players[index];
        nowPlayPlayer.ChangeStat(PlayerStat.Move);
        Debug.Log($"玩家{nowPlayPlayer.Id}開始");
    }
}
