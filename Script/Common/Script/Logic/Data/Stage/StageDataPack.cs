using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tables;

public class StageDataItem
{
    public string StageID;
    public int Star;

    private Tables.StageInfoRecord _StageRecord;
    public StageInfoRecord StageRecord
    {
        get
        {
            if (_StageRecord == null)
            {
                _StageRecord = TableReader.StageInfo.GetRecord(StageID);
            }
            return _StageRecord;
        }
    }
}

public class StageDataPack : DataPackBase
{
    #region 单例

    private static StageDataPack _Instance;
    public static StageDataPack Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new StageDataPack();
            }
            return _Instance;
        }
    }

    private StageDataPack()
    {
        _SaveFileName = "StageDataPack";
    }

    #endregion

    #region 

    [SaveField(1)]
    public List<StageDataItem> _StageItems;
    
    public void InitStageInfo()
    {
        if (_StageItems == null)
        {
            _StageItems = new List<StageDataItem>();

        }

        if (_StageItems.Count == 0)
        { 
            foreach (var tabRecord in TableReader.StageInfo.Records)
            {
                StageDataItem stageItem = new StageDataItem();
                stageItem.StageID = tabRecord.Key;
                stageItem.Star = 0;

                _StageItems.Add(stageItem);
            }
        }
        
    }
    
    #endregion


}