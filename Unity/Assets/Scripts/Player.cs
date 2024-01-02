using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStat
{
    Wait,
    Buy,
    Lose,
    Move,
    PayToOther
}
public class Player : MonoBehaviour
{
    public int Id = 0;
    public long money = 1000000;
    public bool isAuto = false;
    public int moveStep;
    public int nowStep = 0;
    private PlayerStat playerStat;
    void Start()
    {
        playerStat = PlayerStat.Wait;
    }

    void Update()
    {
        if (GameManager.instance.endGame)
            return;
        switch (playerStat)
        {
            case PlayerStat.Wait:
                break;
            case PlayerStat.Buy:
                if (isAuto)
                {
                    GameManager.instance.BuyMap(isAuto);
                    ChangeStat(PlayerStat.Wait);
                }
                break;
            case PlayerStat.Lose:
                break;
            case PlayerStat.Move:
                if (isAuto)
                {
                    OnClick();
                }
                break;
            default:
                break;
        }
    }
    public void OnClick()
    {
        moveStep = Random.Range(2, 13);
    }
    public void ChangeStat(PlayerStat stat)
    {
        playerStat = stat;
    }
}
