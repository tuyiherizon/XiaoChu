using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBallInfo : MonoBehaviour
{
    #region show

    private BallType _BallSPType;
    public BallType BallSPType
    {
        get
        {
            return _BallSPType;
        }
    }

    public virtual void SetBallSPType(BallInfo ballInfo)
    {
        _BallSPType = ballInfo.BallSPType;
    }

    public virtual void ShowBallInfo(BallInfo ballInfo)
    { }
    #endregion
}
