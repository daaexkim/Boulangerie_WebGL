using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : Singleton<GameManager>
{
    [Title("세이브 변수")]
    public string SAVE_PATH;
    public int topScore;
    public int tarCoin;
    [Title("현재 언어")]
    public Country curCountry;

    [Title("현재 게임 난이도")]
    public GameMode gameMode;
    [Title("아이템 가격 (왼쪽부터)")]
    public int[] itemPrices;

    [Title("인게임 변수 (테스트용)")]
    public bool isGameover;
    public int curCoin;
    public int curScore, tarScore;
    private int lastScore = 0;

    protected override void Awake()
    {
        base.Awake();

        LoadAll();
    }

    public void ReSet()
    {
        Time.timeScale = 1f;
        isGameover = false;
        curScore = 0;
        tarScore = 0;
        lastScore = 0;
    }
    public void Save_Score()
    {
        ES3.Save<int>("Score", topScore, SAVE_PATH);
    }
    public void Save_Coin()
    {
        ES3.Save<int>("Coin", tarCoin, SAVE_PATH);
    }
    public void Save_Country()
    {
        ES3.Save<Country>("Country", curCountry, SAVE_PATH);
    }
    public void LoadAll()
    {
        topScore = ES3.Load<int>("Score", SAVE_PATH, 0);
        tarCoin = ES3.Load<int>("Coin", SAVE_PATH, 0);
        curCountry = ES3.Load<Country>("Country", SAVE_PATH, Country.ko);
        curCoin = tarCoin;
    }

    public void GainScore(int amount)
    {
        tarScore += amount;
    }
    public void GainCoin(int amount)
    {
        int calAmount = amount;
        if(amount > 0)
            calAmount = gameMode == GameMode.Bebe ? amount / 2 : amount;

        tarCoin += calAmount;

        Save_Coin();
    }

    public IEnumerator CoinRoutine()
    {
        while (true)
        {
            if (curCoin < tarCoin)
                curCoin += 5;
            else if (curCoin > tarCoin)
                curCoin -= 5;

            yield return new WaitForSeconds(0.025f);
        }
    }
    public IEnumerator ScoreRoutine()
    {
        SpawnManager sm = SpawnManager.Instance;
        UIManager um = UIManager.Instance;

        while (true)
        {
            if (curScore != tarScore)
                curScore++;

            if (curScore % 50 == 0 && curScore != 0 && curScore != lastScore)
            {
                for (int i = 0; i < 5; i++)
                    sm.Spawn_CoinEffect(um.scoreText.transform.position, um.coinTrans.position, true);
                lastScore = curScore;
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void GameOver(Pain overPain)
    {
        if (isGameover)
            return;

        SoundManager.Instance.SFXPlay(SFXType.Over);
        isGameover = true;
        StartCoroutine(GameoverRoutine(overPain));
    }
    private IEnumerator GameoverRoutine(Pain overPain)
    {
        SpawnManager sm = SpawnManager.Instance;
        BtnManager bm = BtnManager.Instance;
        UIManager um = UIManager.Instance;
        List<Pain> liveList = new List<Pain>(sm.painList);

        if (tarScore > topScore)
        {
            topScore = tarScore;
            Save_Score();
        }
        Save_Coin();

        try
        {
            if (overPain != null)
            {
                bool isSeq = false;
                overPain.faceSr.sprite = sm.painFace.fallSprite;
                Sequence seq = DOTween.Sequence().SetUpdate(true);
                seq.Append(overPain.sr.DOColor(Color.red, 0.3f).SetEase(Ease.OutExpo))
                    .Append(overPain.sr.DOColor(Color.white, 0.2f).SetEase(Ease.OutExpo))
                    .Append(overPain.sr.DOColor(Color.red, 0.3f).SetEase(Ease.OutExpo))
                    .Append(overPain.sr.DOColor(Color.white, 0.2f).SetEase(Ease.OutExpo))
                    .Append(overPain.sr.DOColor(Color.red, 0.3f).SetEase(Ease.OutExpo))
                    .Append(overPain.sr.DOColor(Color.white, 0.2f).SetEase(Ease.OutExpo))
                    .OnComplete(() => isSeq = true);

                yield return new WaitUntil(() => isSeq);
            }

            if (sm.newPain != null)
                liveList.Add(sm.newPain);

            foreach (Pain pain in liveList)
            {
                if (pain == null || !pain.gameObject.activeSelf)
                    continue;
                pain.Burned();
                yield return new WaitForSeconds(0.025f);
            }
            yield return new WaitForSeconds(0.2f);

            sm.Spawn_Effect(8f, Vector2.zero, Color.white);

            foreach (Pain pain in liveList)
            {
                sm.Destroy_Pain(pain.level, pain);
            }

            yield return new WaitForSeconds(0.5f);
        }

        finally
        {
            um.gameoverPannel.SetUI(topScore, tarScore, tarCoin);

            bm.Stop();
            bm.Tab_GameOver();
        }
    }
}

public enum GameMode { Bebe = 0, Jeune, Adulte }