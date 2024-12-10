using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;
using DG.Tweening;

public class FireEffect : MonoBehaviour, IPoolObject
{
    SoundManager soundM;
    SpawnManager sm;
    SpriteRenderer sr;
    float defScale;

    public void OnCreatedInPool()
    {
        name = name.Replace("(Clone)", "");

        soundM = SoundManager.Instance;
        sm = SpawnManager.Instance;
        sr = GetComponent<SpriteRenderer>();

        transform.localScale = Vector3.zero;
        sr.color = Color.white;
    }

    public void OnGettingFromPool()
    {
        soundM.SFXPlay(SFXType.Fire);
    }

    public void SetEffect(float scale, Vector2 pos, Pain delPain)
    {
        defScale = scale;
        transform.position = pos;

        Sequence fireSeq = DOTween.Sequence().SetUpdate(true);

        fireSeq.OnStart(() =>
        {
            transform.localScale = Vector3.zero;
            sr.color = Color.white;
        });
        fireSeq.Append(transform.DOScale(Random.Range(defScale - defScale / 2f, defScale + defScale / 2f), 0.5f))
            .Join(sr.DOFade(0.7f, 0.5f).SetEase(Ease.InQuart))
            .OnComplete(() =>
            {
                if(delPain.gameObject.activeSelf)
                    sm.Destroy_Pain(delPain.level, delPain);
                sm.Destroy_FireEffect(this);
            });
    }
}
