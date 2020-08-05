using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallInfoSPLineRowTrap : BallInfoSPBase
{

    public override bool IsCanExchange(BallInfo other)
    {
        return true;
    }

    public override bool IsExchangeSpInfo(BallInfo other)
    {
        return true;
    }

    public override bool IsCanNormalElimit()
    {
        return true;
    }

    public override bool IsCanBeSPElimit(BallInfo other)
    {
        return true;
    }

    public override bool IsContentNormal()
    {
        return true;
    }

    public override bool IsCanFall()
    {
        return true;
    }

    public override bool IsCanMove()
    {
        return true;
    }

    public override bool IsCanPass()
    {
        return true;
    }

    public override bool IsExplore()
    {
        return true;
    }

    public override void OnSPElimit()
    {
        _BallInfo.OnNormalElimit();
    }

    public override List<BallInfo> CheckSPElimit()
    {
        return GetBombBalls();
    }

    public override List<BallInfo> CheckSPMove(List<BallInfo> checkBalls)
    {
        //return GetBombBalls();
        return null;
    }

    public override void SetParam(string[] param)
    {
        if (param.Length > 1)
        {
            _BallInfo.SetBallType((BallType)int.Parse(param[1]));
        }
    }

    private List<BallInfo> GetBombBalls()
    {
        List<BallInfo> bombBalls = new List<BallInfo>();

        //for (int i = 0; i <= BallBox.Instance.BoxWidth; ++i)
        //{
        //    var bombBall = BallBox.Instance.GetBallInfo((int)i, (int)_BallInfo.Pos.y);
        //    if (bombBall != null && bombBall.IsCanBeSPElimit(_BallInfo))
        //    {
        //        bombBalls.Add(bombBall);
        //    }
        //}

        for (int i = (int)_BallInfo.Pos.x; i >= 0; --i)
        {
            var bombBall = BallBox.Instance.GetBallInfo((int)i, (int)_BallInfo.Pos.y);
            if (bombBall == null)
                continue;
            //if (bombBall.BallSPType == BallType.Clod
            //    || bombBall.BallSPType == BallType.Ice
            //    || bombBall.BallSPType == BallType.Stone)
            //    break;
            if (bombBall != null && bombBall.IsCanBeSPElimit(_BallInfo))
            {
                bombBalls.Add(bombBall);
            }
            else
            {
                int tet = 1 + 1;
            }
        }

        for (int i = (int)_BallInfo.Pos.x + 1; i < BallBox.Instance.BoxWidth; ++i)
        {
            var bombBall = BallBox.Instance.GetBallInfo((int)i, (int)_BallInfo.Pos.y);
            if (bombBall == null)
                continue;
            //if (bombBall.BallSPType == BallType.Clod
            //    || bombBall.BallSPType == BallType.Ice
            //    || bombBall.BallSPType == BallType.Stone)
            //    break;
            if (bombBall != null && bombBall.IsCanBeSPElimit(_BallInfo))
            {
                bombBalls.Add(bombBall);
            }
            else
            {
                int tet = 1 + 1;
            }
        }
        //bombBalls.Add(_BallInfo);

        return bombBalls;
    }
}
