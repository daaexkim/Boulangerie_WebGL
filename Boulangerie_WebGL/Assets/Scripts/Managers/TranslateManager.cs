using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TranslateManager : Singleton<TranslateManager>
{
    public TMP_FontAsset font_en, font_ko, font_fr;
    public List<TranslateTxt> translateTxts;
    public List<TranslateImg> translateImgs;
    public List<TranslateRect> translateRects;
    public List<TranslateTxtArea> translateTxtAreas;

    public void Translate_Texts()
    {
        foreach (TranslateTxt text in translateTxts)
            text.Translate();
    }
    public void Translate_Imgs()
    {
        foreach (TranslateImg img in translateImgs)
            img.Translate();
    }
    public void Translate_Rects()
    {
        foreach (TranslateRect rect in translateRects)
            rect.Translate();
    }
    public void Translate_TextAreas()
    {
        foreach (TranslateTxtArea txtArea in translateTxtAreas)
            txtArea.Translate();
    }
}

public enum Country {
    ko, en 
}