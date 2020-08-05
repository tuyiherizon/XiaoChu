using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UIMainFun : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIMainFun, UILayer.BaseUI, hash);
    }

    public static void ShowAsynInFight()
    {
        Hashtable hash = new Hashtable();
        hash.Add("IsInFight", true);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIMainFun, UILayer.BaseUI, hash);
    }

    public static void UpdateMoney()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIMainFun>(UIConfig.UIMainFun);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.UpdateMoneyInner();
    }
    
    public static bool IsUIShow()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIMainFun>(UIConfig.UIMainFun);
        if (instance == null)
            return false;

        if (!instance.isActiveAndEnabled)
            return false;

        return true;
    }
    
    #endregion

    #region 

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        UpdateMoneyInner();


        bool isInFight = false;
        if (hash.ContainsKey("IsInFight"))
        {
            isInFight = (bool)hash["IsInFight"];
        }

    }

    #endregion

    #region info

    public UICurrencyItem _GoldItem;
    public UICurrencyItem _DiamondItem;

    private void UpdateMoneyInner()
    {
        //_GoldItem.ShowOwnCurrency(MONEYTYPE.GOLD);
        //_DiamondItem.ShowOwnCurrency(MONEYTYPE.DIAMOND);
    }

    #endregion

    #region 

    public void OnBtnFight()
    {
        UIFightBox.ShowStage(Tables.TableReader.StageInfo.GetRecord("1"));
        Hide();
    }

    public void OnBtnWeapon()
    {
        UIChangeWeapon.ShowAsyn();
    }

    #endregion
}

