using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tables;



public class PlayerDataPack : DataPackBase
{
    #region 单例

    private static PlayerDataPack _Instance;
    public static PlayerDataPack Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new PlayerDataPack();
            }
            return _Instance;
        }
    }

    private PlayerDataPack()
    {
        _SaveFileName = "PlayerDataPack";
    }

    #endregion

    #region money
    [SaveField(1)]
    private int _Gold = 0;
    public int Gold
    {
        get
        {
            return _Gold;
        }
    }

    [SaveField(2)]
    private int _Diamond = 0;
    public int Diamond
    {
        get
        {
            return _Diamond;
        }
    }

    public void AddGold(int value)
    {
        _Gold += value;
        SaveClass(false);
        UIMainFun.UpdateMoney();
    }

    public bool DecGold(int value)
    {
        if (_Gold < value)
        {
            UIMessageTip.ShowMessageTip(20000);
            return false;
        }

        _Gold -= value;
        SaveClass(false);
        UIMainFun.UpdateMoney();
        return true;
    }

    public void AddDiamond(int value)
    {
        _Diamond += value;
        SaveClass(false);
        UIMainFun.UpdateMoney();
    }

    public bool DecDiamond(int value)
    {
        if (_Diamond < value)
        {
            UIMessageTip.ShowMessageTip(20001);
            return false;
        }

        _Diamond -= value;
        SaveClass(false);
        UIMainFun.UpdateMoney();
        return true;
    }
    #endregion

    
}

