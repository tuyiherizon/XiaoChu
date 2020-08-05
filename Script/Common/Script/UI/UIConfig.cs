using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum UILayer
{
    ControlUI,
    BaseUI,
    MainFunUI,
    PopUI,
    SubPopUI,
    Sub2PopUI,
    MessageUI,
    TopUI
}

public class AssetInfo
{
    public string AssetPath;

    public AssetInfo(string assetPath)
    {
        AssetPath = assetPath;
    }
}

public class UIConfig
{
    public static AssetInfo UILogin = new AssetInfo("SystemUI/UILogin");
    public static AssetInfo UILoadingScene = new AssetInfo("SystemUI/UILoadingScene");
    public static AssetInfo UIMessageBox = new AssetInfo("SystemUI/UIMessageBox");
    public static AssetInfo UISystemSetting = new AssetInfo("SystemUI/UISystemSetting");
    public static AssetInfo UIMessageTip = new AssetInfo("LogicUI/Message/UIMessageTip");
    public static AssetInfo UIDiamondEnsureMsgBox = new AssetInfo("SystemUI/UIDiamondEnsureMsgBox");
    public static AssetInfo UITextTip = new AssetInfo("LogicUI/Message/UITextTip");

    public static AssetInfo UIMainFun = new AssetInfo("LogicUI/UIMainFun");
    public static AssetInfo UILoadingTips = new AssetInfo("LogicUI/UILoadingTips");

    public static AssetInfo UIFightBox = new AssetInfo("LogicUI/Fight/BallGame/UIFightBox");
    public static AssetInfo UIFightBattleField = new AssetInfo("LogicUI/Fight/BattleField/UIFightBattleField");

    public static AssetInfo UIChangeWeapon = new AssetInfo("LogicUI/Weapon/UIChangeWeapon");
    public static AssetInfo UIStageSelect = new AssetInfo("LogicUI/Stage/UIStageSelect");
    public static AssetInfo UIStageEnsure = new AssetInfo("LogicUI/Stage/UIStageEnsure");


}
