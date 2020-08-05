using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tables;

public class WeaponDataItem
{
    public string WeaponID;
    public bool IsGot = false;

    private Tables.WeaponRecord _WeaponRecord;
    public WeaponRecord WeaponRecord
    {
        get
        {
            if (_WeaponRecord == null)
            {
                _WeaponRecord = TableReader.Weapon.GetRecord(WeaponID);
            }
            return _WeaponRecord;
        }
    }
}

public class WeaponDataPack : DataPackBase
{
    #region 单例

    private static WeaponDataPack _Instance;
    public static WeaponDataPack Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new WeaponDataPack();
            }
            return _Instance;
        }
    }

    private WeaponDataPack()
    {
        _SaveFileName = "WeaponDataPack";
    }

    #endregion

    #region 

    [SaveField(1)]
    private string _SelectedWeapon;
    
    public WeaponRecord SelectedWeaponItem
    {
        get
        {
            return TableReader.Weapon.GetRecord(_SelectedWeapon);
        }
    }

    [SaveField(2)]
    public List<string> _GotWeapons;

    public List<WeaponDataItem> WeaponItems;


    public void InitWeaponInfo()
    {
        if (_GotWeapons == null)
        {
            _GotWeapons = new List<string>();
        }
        WeaponItems = new List<WeaponDataItem>();
        foreach (var tabWeaponID in TableReader.Weapon.Records.Keys)
        {
            WeaponDataItem weaponItem = new WeaponDataItem();
            weaponItem.WeaponID = tabWeaponID;
            if (_GotWeapons.Contains(tabWeaponID))
            {
                weaponItem.IsGot = true;
            }
            WeaponItems.Add(weaponItem);
        }
    }

    public void SetSelectWeapon(string weaponID)
    {
        _SelectedWeapon = weaponID;
        SaveClass(true);
    }
    #endregion


}