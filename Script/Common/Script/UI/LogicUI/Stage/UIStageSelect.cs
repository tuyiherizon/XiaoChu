using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tables;

public class UIStageSelect : UIBase
{
    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIStageSelect, UILayer.BaseUI, hash);
    }

    #endregion

    public UIContainerBase _Container;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        Refresh();
    }

    private void Refresh()
    {
        _Container.InitContentItem(StageDataPack.Instance._StageItems, OnChooseStage);
    }

    private void OnChooseStage(object selectItem)
    {
        StageDataItem selectStage = (StageDataItem)selectItem;
        Debug.Log("OnChooseStage:" + selectStage.StageID);
        UIStageEnsure.ShowStageEnsure(selectStage);
    }
}
