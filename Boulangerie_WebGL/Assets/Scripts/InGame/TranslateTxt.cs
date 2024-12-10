using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TranslateTxt : MonoBehaviour
{
    TextMeshProUGUI text;
    public TMP_FontAsset font_ko, font_en;

    public string ko, en;

    private void Start()
    {
        Translate();
    }

    public void Translate()
    {
        text = GetComponent<TextMeshProUGUI>();

        switch (GameManager.Instance.curCountry)
        {
            case Country.ko:
                text.font = font_ko;
                text.text = ko;
                break;
            case Country.en:
                text.font = font_en;
                text.text = en;
                break;
        }
    }
}
