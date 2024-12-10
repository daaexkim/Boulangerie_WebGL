using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager_mm : Singleton<UIManager_mm>
{
    public TextMeshProUGUI coinTxt, topScoreTxt;

    private void Start()
    {
        GameManager gm = GameManager.Instance;

        coinTxt.text = $"<sprite=0>{gm.curCoin}";
        topScoreTxt.text = $"TOP {gm.topScore}";
    }
}
