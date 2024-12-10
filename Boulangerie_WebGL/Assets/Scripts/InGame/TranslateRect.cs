using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TranslateRect : MonoBehaviour
{
    public Vector2 ko, en;

    private void Start()
    {
        Translate();
    }

    public void Translate()
    {
        switch(GameManager.Instance.curCountry)
        {
            case Country.ko:
                GetComponent<RectTransform>().sizeDelta = ko;
                break;
            case Country.en:
                GetComponent<RectTransform>().sizeDelta = en;
                break;
        }
    }
}
