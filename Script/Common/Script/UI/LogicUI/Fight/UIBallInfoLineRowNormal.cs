using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBallInfoLineRowNormal : UIBallInfo
{
    public GameObject _SPShowGO;

    #region show
    public override void ShowBallInfo(BallInfo ballInfo)
    {
        _SPShowGO.SetActive(true);
    }
    #endregion
}
