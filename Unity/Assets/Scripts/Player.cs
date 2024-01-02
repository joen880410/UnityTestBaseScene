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
    private PlayerStat playerStat;
    public Button button1;
    public Text Text_button1;
    public Button button2;
    public Text Text_button2;
    public Text Text_money;
    public Text Text_UID;
    void Start()
    {
        Text_money.text = money.ToString();
        Text_UID.text = Id.ToString();
        playerStat = PlayerStat.Wait;
        button1?.gameObject.SetActive(!isAuto);
        button2?.gameObject.SetActive(!isAuto);
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
        UpdateUI();
    }
    public void UpdateUI()
    {
        Text_money.text = money.ToString();
        Text_UID.text = Id.ToString();
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
