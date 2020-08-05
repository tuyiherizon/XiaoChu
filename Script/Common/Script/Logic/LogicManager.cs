using UnityEngine;
using System.Collections;
using System;

using Tables;
using UnityEngine.SceneManagement;

public class LogicManager
{
    #region 唯一

    private static LogicManager _Instance = null;
    public static LogicManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new LogicManager();
            }
            return _Instance;
        }
    }

    private LogicManager() { }

    #endregion

    #region start logic

    public void StartLoadLogic()
    {
        SceneManager.LoadScene(GameDefine.GAMELOGIC_SCENE_NAME);

        PlayerDataPack.Instance.LoadClass(true);
        PlayerDataPack.Instance.InitPlayerData();

        WeaponDataPack.Instance.LoadClass(true);
        WeaponDataPack.Instance.InitWeaponInfo();

        StageDataPack.Instance.LoadClass(true);
        StageDataPack.Instance.InitStageInfo();
    }

    #endregion

    #region

    public void StartLogic()
    {
        UIStageSelect.ShowAsyn();
        UIMainFun.ShowAsyn();

        PurchManager.Instance.InitIAPInfo();

        GameCore.Instance._SoundManager.PlayBGMusic(GameCore.Instance._SoundManager._LogicAudio);
    }

    public void SaveGame()
    {
        PlayerDataPack.Instance.SaveClass(false);
        
    }

    public void QuitGame()
    {
        try
        {
            SaveGame();
            DataLog.StopLog();
            Application.Quit();
        }
        catch (Exception e)
        {
            Application.Quit();
        }
    }

    public void CleanUpSave()
    {

    }
    #endregion

    #region Fight

    private StageInfoRecord _EnterStageInfo;
    public StageInfoRecord EnterStageInfo
    {
        get
        {
            return _EnterStageInfo;
        }
    }

    public void EnterFight(StageInfoRecord enterStage)
    {
        _EnterStageInfo = enterStage;

        GameCore.Instance.UIManager.DestoryAllUI();

        Hashtable hash = new Hashtable();
        hash.Add("StageRecord", enterStage);

        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_ENTER_STAGE, this, hash);

        var mapRecord = StageMapRecord.ReadStageMap(enterStage.ScenePath[0]);
        BallBox.Instance.Init(mapRecord);
        BallBox.Instance.InitBallInfo();

        BattleField.Instance.InitBattle(enterStage);
    }

    public void EnterFightFinish()
    {
        UIFightBattleField.ShowAsyn();
        UIFightBox.ShowStage(EnterStageInfo);
        GameCore.Instance._SoundManager.PlayBGMusic(EnterStageInfo.Audio);
    }

    public void ExitFight()
    {
        GameCore.Instance.UIManager.DestoryAllUI();

        UIStageSelect.ShowAsyn();
        UIMainFun.ShowAsyn();
    }

    public void ExitFightScene()
    {
        GameCore.Instance.UIManager.DestoryAllUI();
        DestoryFightLogic();
        UILoadingScene.ShowAsyn(GameDefine.GAMELOGIC_SCENE_NAME);

        //UIMainFun.ShowAsynInFight();
    }

    public void DestoryFightLogic()
    {

    }

    public void InitFightScene()
    {
        DestoryFightLogic();

    }


    #endregion
}

