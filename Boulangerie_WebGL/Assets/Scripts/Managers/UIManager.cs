using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UIManager : Singleton<UIManager>
{
    public GameObject raycastPannel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI coinText;
    public RectTransform coinTrans;
    public RectTransform[] itemTrans;
    public TextMeshProUGUI[] itemTexts;
    public GameObject translateImg;
    public GameoverPannel gameoverPannel;
    

    private void Start()
    {
        GameManager gm = GameManager.Instance;
        for(int i=0; i<itemTexts.Length; i++)
        {
            itemTexts[i].text = $"<sprite=0>{gm.itemPrices[i]}";
        }
        if (gm.gameMode == GameMode.Bebe)
            translateImg.SetActive(false);
        StartCoroutine(gm.CoinRoutine());
        StartCoroutine(gm.ScoreRoutine());
    }

    private void Update()
    {
        GameManager gm = GameManager.Instance;

        scoreText.text = gm.curScore.ToString();
        coinText.text = gm.curCoin.ToString();
    }
}

[Serializable]
public struct GameoverPannel
{
    public RectTransform trans;
    public TextMeshProUGUI topscoreText, scoreText, coinText;

    public void SetUI(int topScore, int score, int coin)
    {
        topscoreText.text = $"TOP {topScore}";
        scoreText.text = score.ToString();
        coinText.text = $"<sprite=0> {coin}";
    }
}