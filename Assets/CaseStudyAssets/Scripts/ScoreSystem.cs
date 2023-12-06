using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField]
    private int killCount = 0;
    [SerializeField]
    private int coinScore = 0;
    [SerializeField]
    TextMeshProUGUI killText;
    [SerializeField]
    TextMeshProUGUI coinText;

    void Start()
    {
        coinScore = PlayerPrefs.GetInt("CoinScore", 0);
        UpdateKillCount();
        UpdateCoinCount();
    }

    public void IncreaseKillCount()
    {
        killCount++;
        UpdateKillCount();
    }

    public void IncreaseCoinScore()
    {
        coinScore++;
        PlayerPrefs.SetInt("CoinScore", coinScore);
        UpdateCoinCount();
    }

    private void UpdateKillCount()
    {
        killText.text = killCount.ToString();
    }

    private void UpdateCoinCount()
    {
        coinText.text = coinScore.ToString();
    }
}
