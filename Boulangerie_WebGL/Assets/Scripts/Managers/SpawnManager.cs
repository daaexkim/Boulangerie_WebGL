using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;

public class SpawnManager : Singleton<SpawnManager>
{
    public int MAX_LEVEL;
    public int cur_maxLevel;
    public Pain newPain;
    public List<Pain> painList;
    public float maxSpawnCool;
    public float curSpawnCool;
    public Color[] genderColors;
    public PainFace painFace;

    public void Update()
    {
        if (GameManager.Instance.isGameover)
            return;

        if (curSpawnCool < maxSpawnCool)
            curSpawnCool += Time.deltaTime;

        if (!newPain && curSpawnCool >= maxSpawnCool)
            newPain = Spawn_Pain_Ran();
        else if(newPain && TouchManager.Instance.isTouching)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            mousePos.x = Mathf.Clamp(mousePos.x, newPain.LBorder, newPain.RBorder);
            mousePos.y = ScreenManager.Instance.camBound.Top - 0.5f;
            mousePos.z = 0;
            newPain.transform.position = Vector3.Lerp(newPain.transform.position, mousePos, 0.5f);
        }
        
    }

    #region Pain
    public Pain Spawn_Pain(int level)
    {
        Pain pain = PoolManager.Instance.GetFromPool<Pain>($"Pain_{level}");

        pain.SetPain(level, CSVManager.Instance.Export_RanWord());

        pain.rigid.simulated = false;
        pain.transform.localPosition = Vector2.zero;

        return pain;
    }
    public void Merge_Pain(int level, Vector2 pos)
    {
        GameManager.Instance.GainScore(level * level);

        Pain pain = Spawn_Pain(level);

        painList.Add(pain);

        pain.transform.position = pos;
        pain.rigid.simulated = true;
        pain.SetFace(PainState.Merge);
        pain.isDropped = true;

        if (level <= 5 && level > cur_maxLevel)
            cur_maxLevel = level;
    }
    public Pain Spawn_Pain_Ran()
    {
        Pain ranPain = Spawn_Pain(Random.Range(0, Mathf.Min(cur_maxLevel, 5) + 1));

        return ranPain;
    }

    public void Destroy_Pain(int level, Pain pain)
    {
        painList.Remove(pain);
        PoolManager.Instance.TakeToPool<Pain>($"Pain_{level}", pain);
    }
    #endregion
    #region Effect
    public Effect Spawn_Effect(float size, Vector2 pos, Color color)
    {
        Effect eft = PoolManager.Instance.GetFromPool<Effect>("Effect_Pang");

        eft.SetEffect(size, pos, color);

        return eft;
    }
    public void Destroy_Effect(Effect effect)
    {
        PoolManager.Instance.TakeToPool<Effect>("Effect_Pang", effect);
    }
    #endregion
    public FireEffect Spawn_FireEffect (float size, Vector2 pos, Pain delPain)
    {
        FireEffect eft = PoolManager.Instance.GetFromPool<FireEffect>("Effect_Fire");

        eft.SetEffect(size, pos, delPain);

        return eft;
    }
    public void Destroy_FireEffect(FireEffect eft)
    {
        PoolManager.Instance.TakeToPool<FireEffect>("Effect_Fire", eft);

    }

    public CoinEffect Spawn_CoinEffect(Vector2 startPos, Vector2 targetPos, bool isPlus)
    {
        CoinEffect coin = PoolManager.Instance.GetFromPool<CoinEffect>("Effect_Coin");

        if (isPlus)
            coin.MoveTween_IsPlus(startPos, targetPos);
        else
            coin.MoveTween_IsMinus(startPos, targetPos);

        return coin;
    }
    public void Destroy_CoinEffect(CoinEffect eft)
    {
        PoolManager.Instance.TakeToPool<CoinEffect>("Effect_Coin", eft);
    }
}

[System.Serializable]
public struct PainFace
{
    public Sprite[] defSprites;
    public Sprite hitSprite;
    public Sprite fallSprite;
    public Sprite mergeSprite;

    public Sprite Export_defSprite_Ran()
    {
        return defSprites[Random.Range(0, defSprites.Length)];
    }
}
