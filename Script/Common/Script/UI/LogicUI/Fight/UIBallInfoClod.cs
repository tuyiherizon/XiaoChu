using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBallInfoClod : UIBallInfo
{
    public GameObject[] _SPBallEarth;
    public Text _EarthNum;

    #region show
    public override void ShowBallInfo(BallInfo ballInfo)
    {
        int spNum = ((BallInfoSPTrapClod)ballInfo._BallInfoSP).ElimitNum;
        int showIdx = 0;
        if (spNum > 2)
        {
            showIdx = 1;
        }
        else
        {
            showIdx = 0;
        }
        for (int i = 0; i < _SPBallEarth.Length; ++i)
        {
            if (i == showIdx)
            {
                _SPBallEarth[i].SetActive(true);
            }
            else
            {
                _SPBallEarth[i].SetActive(false);
            }
        }
        _EarthNum.text = spNum.ToString();
    }
    #endregion
}
