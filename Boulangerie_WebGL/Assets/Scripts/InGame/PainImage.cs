using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Redcode.Pools;
using UnityEngine.UI;

public class PainImage : MonoBehaviour, IPoolObject
{
    public float speed;
    public float startX;

    SpawnManager_mm sm;
    Image image;

    void IPoolObject.OnCreatedInPool()
    {
        sm = SpawnManager_mm.Instance;
        image = GetComponent<Image>();
    }

    void IPoolObject.OnGettingFromPool()
    {
        SetImage_Ran();
    }

    private void FixedUpdate()
    {
        Vector2 currentPosition = image.rectTransform.anchoredPosition;

        // 새로운 위치 계산 (x값 증가)
        Vector2 newPosition = currentPosition + new Vector2(speed * Time.deltaTime, 0);

        if (newPosition.x > -startX + sm.spacing)
        {
            newPosition.x = startX - sm.spacing;
        }

        // 위치 업데이트
        image.rectTransform.anchoredPosition = newPosition;
    }

    public void SetImg(Vector2 pos, float _speed, float _startX)
    {
        image.rectTransform.anchoredPosition = pos;
        speed = _speed;
        startX = _startX;
    }

    private void SetImage_Ran()
    {
        image.sprite = sm.painImages[Random.Range(0, sm.painImages.Length)];

        int ranSize = Random.Range(160, 221);
        image.rectTransform.sizeDelta = new Vector2(ranSize, ranSize);
    }
}
