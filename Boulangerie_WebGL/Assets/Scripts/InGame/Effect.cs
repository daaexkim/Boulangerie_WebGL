using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;

public class Effect : MonoBehaviour, IPoolObject
{
    SpriteRenderer sr;
    SpawnManager sm;
    SoundManager soundM;
    public void OnCreatedInPool()
    {
        name = name.Replace("(Clone)", "");

        sr = GetComponent<SpriteRenderer>();
        sm = SpawnManager.Instance;
        soundM = SoundManager.Instance;
    }

    public void OnGettingFromPool()
    {
        soundM.SFXPlay(SFXType.Pop);
    }

    public void SetEffect(float size, Vector2 pos, Color color)
    {
        transform.localScale = new Vector2(size, size);
        transform.position = pos;
        sr.color = color;
    }

    public void Destroy()
    {
        sm.Destroy_Effect(this);
    }
}
