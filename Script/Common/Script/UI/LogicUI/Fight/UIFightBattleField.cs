using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tables;

public class UIFightBattleField : UIBase
{
    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIFightBattleField, UILayer.BaseUI, hash);
    }

    #endregion

    public BattleScene _BattleScene;
    public RawImage _BattleImage;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

    }
    
}
