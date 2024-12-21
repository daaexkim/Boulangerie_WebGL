using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnManager : Singleton<BtnManager>
{
    public bool isIteming;
    public GameObject rayCastPannel;
    public void Tab(GameObject obj)
    {
        if (TouchManager.Instance.isTouching)
            return;

        if (!obj.activeSelf)
        {
            obj.SetActive(true);
            rayCastPannel.SetActive(true);
            obj.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            obj.transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f).SetEase(Ease.InExpo).SetEase(Ease.OutBounce).SetUpdate(true);
        }
        else
        {
            rayCastPannel.SetActive(false);
            obj.transform.DOScale(new Vector3(0.05f, 0.05f, 0.05f), 0.25f).SetEase(Ease.InOutExpo).SetUpdate(true).OnComplete(() => obj.SetActive(false));
        }
    }
    public void Tab_NoRayCast(GameObject obj)
    {
        if (TouchManager.Instance.isTouching)
            return;

        if (!obj.activeSelf)
        {
            obj.SetActive(true);
            obj.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            obj.transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f).SetEase(Ease.InExpo).SetEase(Ease.OutBounce).SetUpdate(true);
        }
        else
        {
            obj.transform.DOScale(new Vector3(0.05f, 0.05f, 0.05f), 0.25f).SetEase(Ease.InOutExpo).SetUpdate(true).OnComplete(() => obj.SetActive(false));
        }
    }
    public void Tab_Pause(GameObject obj)
    {
        if (TouchManager.Instance.isTouching || GameManager.Instance.isGameover)
            return;

        if (!obj.activeSelf)
        {
            obj.SetActive(true);
            rayCastPannel.SetActive(true);
            obj.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            obj.transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f).SetEase(Ease.InExpo).SetEase(Ease.OutBounce).SetUpdate(true);
        }
        else
        {
            rayCastPannel.SetActive(false);
            obj.transform.DOScale(new Vector3(0.05f, 0.05f, 0.05f), 0.25f).SetEase(Ease.InOutExpo).SetUpdate(true).OnComplete(() => obj.SetActive(false));
        }
    }
    public void Tab_GameOver()
    {
        GameObject obj = UIManager.Instance.gameoverPannel.trans.gameObject;

        obj.SetActive(true);
        rayCastPannel.SetActive(true);
        obj.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        obj.transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f).SetEase(Ease.InExpo).SetEase(Ease.OutBounce).SetUpdate(true);
    }

    bool isStartTab;
    public void TabToStartBtn()
    {
        if (isStartTab)
            return;

        isStartTab = true;
        Screen.fullScreen = !Screen.fullScreen;
        Application.targetFrameRate = 60;
        // SoundManager.Instance.BGMPlay(3);

        SceneManager.LoadScene(1);
    }

    public void Stop()
    {
        Time.timeScale = 0f;
    }
    public void Resume()
    {
        Time.timeScale = 1f;
    }

    public void Start_Game(int modeID)
    {
        GameManager.Instance.gameMode = (GameMode)modeID;
        // SoundManager.Instance.BGMPlay(modeID);
        SceneManager.LoadScene(2);
    }
    public void Back_Menu()
    {
        GameManager.Instance.ReSet();
        // SoundManager.Instance.BGMPlay(3);

        SceneManager.LoadScene(1);
    }
    public void Restart_Game()
    {
        GameManager.Instance.ReSet();
        // SoundManager.Instance.bgmPlayer.Play();

        SceneManager.LoadScene(2);
    }
    public void GameOverBtn(GameObject obj)
    {
        Tab(obj);
        GameManager.Instance.GameOver(null);
    }

    public void TranslateDown() {
        if (GameManager.Instance.isGameover)
            return;

        SpawnManager sm = SpawnManager.Instance;
        foreach(Pain pain in sm.painList)
        {
            pain.Translation();
        }
        if(sm.newPain)
            sm.newPain.Translation();
    }
    public void TranslateUp()
    {
        if (GameManager.Instance.isGameover)
            return;

        SpawnManager sm = SpawnManager.Instance;
        foreach (Pain pain in sm.painList)
        {
            pain.ResetTranslation();
        }
        if(sm.newPain)
            sm.newPain.ResetTranslation();
    }
    public void TranslateBtn()
    {
        TranslateManager tm = TranslateManager.Instance;
        GameManager gm = GameManager.Instance;

        // Country 열거형 값의 수를 얻어 wrap-around 방식으로 다음 값을 계산
        int nextCountryID = ((int)gm.curCountry + 1) % System.Enum.GetValues(typeof(Country)).Length;

        // 현재 국가 업데이트
        gm.curCountry = (Country)nextCountryID;

        gm.Save_Country();

        // 번역 실행
        tm.Translate_Texts();
        tm.Translate_Imgs();
        tm.Translate_Rects();
        tm.Translate_TextAreas();
    }

    public void MixBtn()
    {
        if (GameManager.Instance.isGameover || isIteming)
            return;

        SpawnManager sm = SpawnManager.Instance;
        List<Pain> curList = new List<Pain>(sm.painList);

        if (curList.Count < 2)
            return;

        if (!ItemCoin(0))
            return;

        StartCoroutine(MixRoutine(curList));
    }
    private IEnumerator MixRoutine(List<Pain> curList)
    {
        isIteming = true;

        SpawnManager sm = SpawnManager.Instance;

        Pain aPain = null;
        Pain bPain = null;

        int loopCount = curList.Count / 2;

        for (int i = 0; i < loopCount; i++)
        {
            if (curList.Count < 2)
                break;

            aPain = curList[Random.Range(0, curList.Count)];
            if (aPain == null || !aPain.gameObject.activeSelf)
            {
                curList.Remove(aPain);
                continue;
            }
            aPain.SetFace(PainState.Merge);
            curList.Remove(aPain);
            bPain = curList[Random.Range(0, curList.Count)];
            if (bPain == null || !bPain.gameObject.activeSelf)
            {
                curList.Remove(bPain);
                continue;
            }
            bPain.SetFace(PainState.Merge);
            curList.Remove(bPain);

            Vector2 aPos = aPain.transform.position;
            Vector2 bPos = bPain.transform.position;
            aPain.transform.position = bPos;
            bPain.transform.position = aPos;

            sm.Spawn_Effect(aPain.defScale + 1f, bPos, Color.white);
            sm.Spawn_Effect(bPain.defScale + 1f, aPos, Color.white);

            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(0.2f);
        isIteming = false;
    }

    public void FireBtn()
    {
        if (GameManager.Instance.isGameover || isIteming)
            return;

        SpawnManager sm = SpawnManager.Instance;
        List<Pain> painList = sm.painList;

        if (painList.Count <= 0)
            return;

        if (!ItemCoin(1))
            return;

        StartCoroutine(FireRoutine(painList));
    }
    private IEnumerator FireRoutine(List<Pain> painList)
    {
        isIteming = true;
        SpawnManager sm = SpawnManager.Instance;

        int maxLv = 0;
        foreach (Pain pain in painList)
        {
            if (pain.level > maxLv)
                maxLv = pain.level;
        }

        List<Pain> tarPains = painList.FindAll(data => data.level == maxLv);
        Pain tarPain = tarPains[Random.Range(0, tarPains.Count)];
        tarPain.Burned();

        List<Vector2> poses = new List<Vector2>();
        int count = Random.Range(2, 5);
        for (int i = 0; i < count; i++)
            poses.Add(tarPain.transform.position + new Vector3(-tarPain.defScale / 2f + tarPain.defScale / 2f / count + (i * tarPain.defScale / count),
                Random.Range(-tarPain.defScale / count, tarPain.defScale / count)));

        poses = poses.OrderBy(x => Random.value).ToList();

        foreach (Vector2 pos in poses)
        {
            sm.Spawn_FireEffect(tarPain.defScale, pos, tarPain);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.2f);
        isIteming = false;
    }

    public void TaupeBtn()
    {
        if (GameManager.Instance.isGameover || isIteming)
            return;


        SpawnManager sm = SpawnManager.Instance;
        List<Pain> availPains = new List<Pain>();

        int maxLv = 0;
        foreach (Pain pain in sm.painList)
        {
            if (pain.isBited)
                continue;
            if (pain.level > maxLv)
            {
                availPains.Add(pain);
                maxLv = pain.level;
            }
        }

        if (availPains.Count <= 0)
            return;

        if (!ItemCoin(2))
            return;

        List<Pain> tarPains = availPains.FindAll(data => data.level == maxLv);
        Pain tarPain = tarPains[Random.Range(0, tarPains.Count)];
        tarPain.SetFace(PainState.Fall);

        tarPain.Bited(true);

    }

    private bool ItemCoin(int id)
    {
        GameManager gm = GameManager.Instance;
        int price = gm.itemPrices[id];

        if(gm.tarCoin >= price)
        {
            SpawnManager sm = SpawnManager.Instance;
            UIManager um = UIManager.Instance;
            for (int i = 0; i < price / 10; i++)
                sm.Spawn_CoinEffect(um.coinTrans.position, um.itemTrans[id].position, false);
            return true;
        }

        return false;
    }

    public void BtnSound()
    {
        SoundManager.Instance.SFXPlay(SFXType.Button);
    }

    public void TestBtn()
    {
        StartCoroutine(TestRoutine());
    }
    private IEnumerator TestRoutine()
    {
        SpawnManager sm = SpawnManager.Instance;
        ScreenManager.Instance.lineTrans.gameObject.SetActive(false);

        for (int i = 0; i <= 10; i++)
        {
            Pain testPain = sm.Spawn_Pain(i);
            testPain.rigid.simulated = true;

            yield return new WaitForSeconds(0.5f);
        }
    }
}
