using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TranslateImg : MonoBehaviour
{
    Image img;
    public Sprite ko, en;

    private void Awake()
    {
        img = GetComponent<Image>();
    }

    private void Start()
    {
        Translate();
    }

    public void Translate()
    {
        switch (GameManager.Instance.curCountry)
        {
            case Country.ko:
                img.sprite = ko;
                break;
            case Country.en:
                img.sprite = en;
                break;
        }
    }
}
