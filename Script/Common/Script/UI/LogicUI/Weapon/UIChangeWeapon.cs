using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tables;

public class UIChangeWeapon : UIBase
{
    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIChangeWeapon, UILayer.PopUI, hash);
    }

    #endregion

    public UIContainerBase _WeaponContainer;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        RefreshWeapons();
    }

    private void RefreshWeapons()
    {
        _WeaponContainer.InitContentItem(WeaponDataPack.Instance.WeaponItems, OnChooseWeapon);
    }

    private void OnChooseWeapon(object selectItem)
    {
        WeaponDataItem chooseWeapon = (WeaponDataItem)selectItem;
        Debug.Log("OnChooseWeapon:" + chooseWeapon.WeaponID);

        WeaponDataPack.Instance.SetSelectWeapon(chooseWeapon.WeaponID);
    }
}
