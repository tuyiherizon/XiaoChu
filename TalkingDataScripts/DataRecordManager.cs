using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;

public class DataRecordManager
{
    #region 唯一

    private static DataRecordManager _Instance = null;
    public static DataRecordManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new DataRecordManager();
            }
            return _Instance;
        }
    }

    private DataRecordManager() { }

    #endregion

    TDGAAccount account;

    public void InitDataRecord()
    {
        TalkingDataGA.OnStart("6825B11C1864443284573062E27D0463", "ggp");
        account = TDGAAccount.SetAccount(TalkingDataGA.GetDeviceId());
        
    }
    
}
