using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBallInfoBombSmallReact : UIBallInfo
{
    public GameObject _SPShowGO;
    public GameObject _SPShowReactGO;

    #region show
    public override void ShowBallInfo(BallInfo ballInfo)
    {
        var reactBall = (BallInfoSPBombSmallReact)ballInfo._BallInfoSP;
        if (reactBall != null)
        {
            if (reactBall._IsReactBall)
            {
                _SPShowGO.SetActive(false);
                _SPShowReactGO.SetActive(true);
            }
            else
            {
                _SPShowGO.SetActive(true);
                _SPShowReactGO.SetActive(false);
            }
        }
    }
    #endregion
}
