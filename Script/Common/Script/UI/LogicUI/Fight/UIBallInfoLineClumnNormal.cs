﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBallInfoLineClumnNormal : UIBallInfo
{
    public GameObject _SPShowGO;

    #region show
    public override void ShowBallInfo(BallInfo ballInfo)
    {
        _SPShowGO.SetActive(true);
    }
    #endregion
}