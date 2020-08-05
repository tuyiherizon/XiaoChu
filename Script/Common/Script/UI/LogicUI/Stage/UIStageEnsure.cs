using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tables;

public class UIStageEnsure : UIBase
{
    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIStageEnsure, UILayer.PopUI, hash);
    }

    public static void ShowStageEnsure(StageDataItem stageInfo)
    {
        Hashtable hash = new Hashtable();
        hash.Add("StageInfo", stageInfo);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIStageEnsure, UILayer.PopUI, hash);
    }

    #endregion

    private StageDataItem _StageInfo;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);
        _StageInfo = (StageDataItem)hash["StageInfo"];
        Refresh();
    }

    private void Refresh()
    {
        
    }

    public void OnOK()
    {
        LogicManager.Instance.EnterFight(_StageInfo.StageRecord);
    }
}
