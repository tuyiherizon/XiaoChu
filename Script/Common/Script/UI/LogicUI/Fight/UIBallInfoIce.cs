using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBallInfoIce : UIBallInfo
{
    public GameObject[] _SPBallFrozen;
    public Text _FrozenNum;

    #region show
    public override void ShowBallInfo(BallInfo ballInfo)
    {
        int spNum = ((BallInfoSPTrapIce)ballInfo._BallInfoSP).ElimitNum;
        int showIdx = 0;
        if (spNum > 2)
        {
            showIdx = 1;
        }
        else
        {
            showIdx = 0;
        }
        for (int i = 0; i < _SPBallFrozen.Length; ++i)
        {
            if (i == showIdx)
            {
                _SPBallFrozen[i].SetActive(true);
            }
            else
            {
                _SPBallFrozen[i].SetActive(false);
            }
        }
        _FrozenNum.text = spNum.ToString();

    }
    #endregion
}
