using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBallInfoNormal : UIBallInfo
{
    #region show

    public GameObject _NormalBall1;
    public GameObject _NormalBall2;
    public GameObject _NormalBall3;
    public GameObject _NormalBall4;
    public GameObject _NormalBall5;
    public GameObject _NormalBallEmpty;

    public Text _BallText;

    public Animation _BallAnim;
    private RectTransform _RectTransform;
    public RectTransform RectTransform
    {
        get
        {
            if (_RectTransform == null)
            {
                _RectTransform = gameObject.GetComponent<RectTransform>();
            }

            return _RectTransform;
        }
    }

    public void ClearBall()
    {
        _NormalBall1.SetActive(false);
        _NormalBall2.SetActive(false);
        _NormalBall3.SetActive(false);
        _NormalBall4.SetActive(false);
        _NormalBall5.SetActive(false);
        _NormalBallEmpty.SetActive(false);
    }

    public override void ShowBallInfo(BallInfo ballInfo)
    {
        switch (ballInfo.BallType)
        {
            case BallType.Color1:
                ClearBall();
                _NormalBall1.SetActive(true);
                break;
            case BallType.Color2:
                ClearBall();
                _NormalBall2.SetActive(true);
                break;
            case BallType.Color3:
                ClearBall();
                _NormalBall3.SetActive(true);
                break;
            case BallType.Color4:
                ClearBall();
                _NormalBall4.SetActive(true);
                break;
            case BallType.Color5:
                ClearBall();
                _NormalBall5.SetActive(true);
                break;
            case BallType.ColorEmpty:
                ClearBall();
                _NormalBallEmpty.SetActive(true);
                break;
            case BallType.None:
                ClearBall();
                break;
        }

        //_BallText.text = ((int)ballInfo.BallType).ToString();
        _BallText.text = (int)ballInfo.Pos.x + "," + (int)ballInfo.Pos.y;
    }


    #endregion
}
