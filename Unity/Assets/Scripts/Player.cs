using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public PlayerStat playerStat { private set; get; } = PlayerStat.Wait;
    public Button button1;
    public Text Text_button1;
    public Button button2;
    public Text Text_button2;
    public Text Text_money;
    public Text Text_UID;
    public Text Text_Auto;

    #region -- 初始化/運作 --

    void Awake()
    {
        Text_Auto.text = isAuto ? "自動" : "手動";
        Text_money.text = money.ToString();
        Text_UID.text = Id.ToString();
        button1?.gameObject.SetActive(!isAuto);
        button2?.gameObject.SetActive(!isAuto);
        ChangeStat(playerStat);
    }

    void Update()
    {
        if (true)
        {
            Text_Auto.text = playerStat.ToString();
            if (GameManager.instance.gameStatus == GameManager.GameStatus.End)
                return;

            switch (playerStat)
            {
                case PlayerStat.Wait:
                    moveStep = 0;
                    break;
                case PlayerStat.Buy:
                    if (isAuto)
                    {
                        GameManager.instance.BuyMap(this, isAuto);
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
            UpdateUI();
        }
    }

    #endregion

    #region --  方法參考區 --

    public void UpdateUI()
    {
        Text_money.text = money.ToString();
        Text_UID.text = Id.ToString();
    }
    public void OnClick()
    {
        moveStep = UnityEngine.Random.Range(2, 13);
        nowStep += moveStep;
        GameManager.instance.PlayerMove(this);

    }
    public void ChangeStat(PlayerStat stat)
    {
        if (!isAuto && stat != PlayerStat.Wait)
        {
            button1.onClick.RemoveAllListeners();
            button2.onClick.RemoveAllListeners();
        }
        playerStat = stat;
        switch (playerStat)
        {
            case PlayerStat.Wait:
                button1?.gameObject.SetActive(false);
                button2?.gameObject.SetActive(false);
                break;
            case PlayerStat.Buy:
                if (!isAuto)
                {
                    if (GameManager.instance.CanBuy(this))
                    {
                        Text_button1.text = "購買地圖";
                        Text_button2.text = "不購買地圖";
                        button1?.gameObject.SetActive(true);
                        button2?.gameObject.SetActive(true);
                        button1.onClick.AddListener(() => { GameManager.instance.BuyMap(this, true); });
                        button2.onClick.AddListener(() => { GameManager.instance.BuyMap(this, false); });
                    }
                    else
                    {
                        GameManager.instance.ChangeNext();
                    }

                }
                break;
            case PlayerStat.Lose:
                break;
            case PlayerStat.Move:
                if (!isAuto)
                {
                    button2.gameObject.SetActive(false);
                    Text_button1.text = "移動";
                    button1?.gameObject.SetActive(true);
                    button1.onClick.AddListener(OnClick);
                }
                break;
            case PlayerStat.PayToOther:
                break;
            default:
                break;
        }
    }

    #endregion
}
