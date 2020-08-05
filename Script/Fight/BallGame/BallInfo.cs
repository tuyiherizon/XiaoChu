using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BallType
{
    None = 0,

    NormalBallStart = 1,
    Color1,
    Color2,
    Color3,
    Color4,
    Color5,
    ColorEmpty,
    NormalBallEnd,

    SPTrapStart = 10,
    Ice,
    Clod,
    Stone,
    Posion,
    Iron,
    SPTrapEnd,

    SPBombStart = 100,
    BombSmall1,
    BombBig1,
    BombSmallEnlarge1,
    BombBigEnlarge,
    BombSmallHitTrap,
    BombBigHitTrap,
    BombSmallReact,
    BombBigReact,
    BombSmallLighting,
    BombBigLighting,
    BombSmallAuto,
    BombBigAuto,
    SPBombEnd,

    SPLineStart = 200,
    LineRow,
    LineClumn,
    LineCross,
    LineRowEnlarge,
    LineClumnEnlarge,
    LineCrossEnlarge,
    LineRowHitTrap,
    LineClumnHitTrap,
    LineCrossHitTrap,
    LineRowReact,
    LineClumnReact,
    LineCrossReact,
    LineRowLighting,
    LineClumnLighting,
    LineCrossLighting,
    LineRowAuto,
    LineClumnAuto,
    LineCrossAuto,
    SPLineEnd,
    
}

public class BallInfo
{
    private BallType _BallType;
    public BallType BallType
    {
        get
        {
            return _BallType;
        }
        set
        {
            _BallType = value;
        }
    }

    private BallType _BallSPType;
    public BallType BallSPType
    {
        get
        {
            return _BallSPType;
        }
        set
        {
            _BallSPType = value;
        }
    }
    public BallInfoSPBase _BallInfoSP = null;

    private Vector2 _Pos;
    public Vector2 Pos
    {
        get
        {
            return _Pos;
        }
    }

    public BallInfo(int posX, int posY)
    {
        _Pos = new Vector2(posX, posY);
        _BallType = BallType.None;
        _BallSPType = BallType.None;
        _BallInfoSP = null;
    }

    public void Clear()
    {
        _BallType = BallType.None;
        _BallSPType = BallType.None;
        _ElimitFlag = 0;
        _BallInfoSP = null;
    }

    #region normal

    public bool IsContainNormal()
    {
        return _BallType > BallType.NormalBallStart && _BallType < BallType.NormalBallEnd;
    }

    public bool IsEmpty()
    {
        return _BallInfoSP == null && _BallType == BallType.None;
    }

    public bool IsNormalBall()
    {
        return _BallType > BallType.NormalBallStart && _BallType < BallType.NormalBallEnd && _BallInfoSP == null;
    }

    public bool IsEquipNormal(BallInfo other)
    {
        if (other == null)
            return false;
        if (_BallType == BallType.None)
            return false;
        return _BallType == other.BallType;
    }

    public bool IsCanFillNormal()
    {
        if (_BallInfoSP != null && !_BallInfoSP.IsContentNormal())
        {
            return false;
        }

        return _BallType == BallType.None;
    }

    public void SetBallType(BallType ballType)
    {
        _BallType = ballType;
    }

    public void SetBallInitType(string ballSPType)
    {
        var spArgs = ballSPType.Split(',');
        if (spArgs.Length < 1)
            return;

        int spType = int.Parse(spArgs[0]);
        if (!BallBox.IsTypeSP(spType))
        {
            _BallType = (BallType)spType;
            SetBallType(_BallType);
        }
        else
        {

            _BallSPType = (BallType)spType;
            switch (_BallSPType)
            {
                case BallType.Ice:
                    _BallInfoSP = new BallInfoSPTrapIce();
                    _BallInfoSP.SetBallInfo(this);
                    _BallInfoSP.SetParam(spArgs);
                    break;
                case BallType.Clod:
                    _BallInfoSP = new BallInfoSPTrapClod();
                    _BallInfoSP.SetBallInfo(this);
                    _BallInfoSP.SetParam(spArgs);
                    break;
                case BallType.Stone:
                    _BallInfoSP = new BallInfoSPTrapStone();
                    _BallInfoSP.SetBallInfo(this);
                    _BallInfoSP.SetParam(spArgs);
                    break;
                case BallType.Posion:
                    _BallInfoSP = new BallInfoSPTrapPosion();
                    _BallInfoSP.SetBallInfo(this);
                    _BallInfoSP.SetParam(spArgs);
                    break;
                case BallType.Iron:
                    _BallInfoSP = new BallInfoSPTrapIron();
                    _BallInfoSP.SetBallInfo(this);
                    _BallInfoSP.SetParam(spArgs);
                    break;
                case BallType.BombSmall1:
                    _BallInfoSP = new BallInfoSPBombSmall();
                    _BallInfoSP.SetBallInfo(this);
                    _BallInfoSP.SetParam(spArgs);
                    break;
                case BallType.BombBig1:
                    _BallInfoSP = new BallInfoSPBombBig();
                    _BallInfoSP.SetBallInfo(this);
                    _BallInfoSP.SetParam(spArgs);
                    break;
                case BallType.BombSmallEnlarge1:
                    _BallInfoSP = new BallInfoSPBombSmallEnlarge();
                    _BallInfoSP.SetBallInfo(this);
                    _BallInfoSP.SetParam(spArgs);
                    break;
                case BallType.BombBigEnlarge:
                    _BallInfoSP = new BallInfoSPBombBigEnlarge();
                    _BallInfoSP.SetBallInfo(this);
                    _BallInfoSP.SetParam(spArgs);
                    break;
                case BallType.BombSmallHitTrap:
                    _BallInfoSP = new BallInfoSPBombSmallTrap();
                    _BallInfoSP.SetBallInfo(this);
                    _BallInfoSP.SetParam(spArgs);
                    break;
                case BallType.BombBigHitTrap:
                    _BallInfoSP = new BallInfoSPBombBigTrap();
                    _BallInfoSP.SetBallInfo(this);
                    _BallInfoSP.SetParam(spArgs);
                    break;
                case BallType.BombSmallReact:
                    _BallInfoSP = new BallInfoSPBombSmallReact();
                    _BallInfoSP.SetBallInfo(this);
                    _BallInfoSP.SetParam(spArgs);
                    break;
                case BallType.BombBigReact:
                    _BallInfoSP = new BallInfoSPBombBigReact();
                    _BallInfoSP.SetBallInfo(this);
                    _BallInfoSP.SetParam(spArgs);
                    break;
                case BallType.BombSmallLighting:
                    _BallInfoSP = new BallInfoSPBombSmallLighting();
                    _BallInfoSP.SetBallInfo(this);
                    _BallInfoSP.SetParam(spArgs);
                    break;
                case BallType.BombBigLighting:
                    _BallInfoSP = new BallInfoSPBombBigLighting();
                    _BallInfoSP.SetBallInfo(this);
                    _BallInfoSP.SetParam(spArgs);
                    break;
                case BallType.BombSmallAuto:
                    _BallInfoSP = new BallInfoSPBombSmallAuto();
                    _BallInfoSP.SetBallInfo(this);
                    _BallInfoSP.SetParam(spArgs);
                    break;
                case BallType.BombBigAuto:
                    _BallInfoSP = new BallInfoSPBombBigAuto();
                    _BallInfoSP.SetBallInfo(this);
                    _BallInfoSP.SetParam(spArgs);
                    break;
                case BallType.LineRow:
                    _BallInfoSP = new BallInfoSPLineRow();
                    _BallInfoSP.SetBallInfo(this);
                    _BallInfoSP.SetParam(spArgs);
                    break;
                case BallType.LineClumn:
                    _BallInfoSP = new BallInfoSPLineClumn();
                    _BallInfoSP.SetBallInfo(this);
                    _BallInfoSP.SetParam(spArgs);
                    break;
                case BallType.LineCross:
                    _BallInfoSP = new BallInfoSPLineCross();
                    _BallInfoSP.SetBallInfo(this);
                    _BallInfoSP.SetParam(spArgs);
                    break;
                case BallType.LineRowEnlarge:
                    _BallInfoSP = new BallInfoSPLineRowEnlarge();
                    _BallInfoSP.SetBallInfo(this);
                    _BallInfoSP.SetParam(spArgs);
                    break;
                case BallType.LineClumnEnlarge:
                    _BallInfoSP = new BallInfoSPLineClumnEnlarge();
                    _BallInfoSP.SetBallInfo(this);
                    _BallInfoSP.SetParam(spArgs);
                    break;
                case BallType.LineCrossEnlarge:
                    _BallInfoSP = new BallInfoSPLineCrossEnlarge();
                    _BallInfoSP.SetBallInfo(this);
                    _BallInfoSP.SetParam(spArgs);
                    break;
                case BallType.LineRowHitTrap:
                    _BallInfoSP = new BallInfoSPLineRowTrap();
                    _BallInfoSP.SetBallInfo(this);
                    _BallInfoSP.SetParam(spArgs);
                    break;
                case BallType.LineClumnHitTrap:
                    _BallInfoSP = new BallInfoSPLineClumnTrap();
                    _BallInfoSP.SetBallInfo(this);
                    _BallInfoSP.SetParam(spArgs);
                    break;
                case BallType.LineCrossHitTrap:
                    _BallInfoSP = new BallInfoSPLineCrossTrap();
                    _BallInfoSP.SetBallInfo(this);
                    _BallInfoSP.SetParam(spArgs);
                    break;
                case BallType.LineRowReact:
                    _BallInfoSP = new BallInfoSPLineRowReact();
                    _BallInfoSP.SetBallInfo(this);
                    _BallInfoSP.SetParam(spArgs);
                    break;
                case BallType.LineClumnReact:
                    _BallInfoSP = new BallInfoSPLineClumnReact();
                    _BallInfoSP.SetBallInfo(this);
                    _BallInfoSP.SetParam(spArgs);
                    break;
                case BallType.LineCrossReact:
                    _BallInfoSP = new BallInfoSPLineCrossReact();
                    _BallInfoSP.SetBallInfo(this);
                    _BallInfoSP.SetParam(spArgs);
                    break;
                case BallType.LineRowLighting:
                    _BallInfoSP = new BallInfoSPLineRowLighting();
                    _BallInfoSP.SetBallInfo(this);
                    _BallInfoSP.SetParam(spArgs);
                    break;
                case BallType.LineClumnLighting:
                    _BallInfoSP = new BallInfoSPLineClumnLighting();
                    _BallInfoSP.SetBallInfo(this);
                    _BallInfoSP.SetParam(spArgs);
                    break;
                case BallType.LineCrossLighting:
                    _BallInfoSP = new BallInfoSPLineCrossLighting();
                    _BallInfoSP.SetBallInfo(this);
                    _BallInfoSP.SetParam(spArgs);
                    break;
                case BallType.LineRowAuto:
                    _BallInfoSP = new BallInfoSPLineRowAuto();
                    _BallInfoSP.SetBallInfo(this);
                    _BallInfoSP.SetParam(spArgs);
                    break;
                case BallType.LineClumnAuto:
                    _BallInfoSP = new BallInfoSPLineClumnAuto();
                    _BallInfoSP.SetBallInfo(this);
                    _BallInfoSP.SetParam(spArgs);
                    break;
                case BallType.LineCrossAuto:
                    _BallInfoSP = new BallInfoSPLineCrossAuto();
                    _BallInfoSP.SetBallInfo(this);
                    _BallInfoSP.SetParam(spArgs);
                    break;
            }

            if (!_BallInfoSP.IsContentNormal())
            {
                SetBallType(BallType.None);
            }
        }
    }

    public void SetRandomBall()
    {
        int ballTypeEnd = (int)BallType.NormalBallEnd;
        if (!BallBox.Instance._IsContainEmptyNormal)
        {
            ballTypeEnd = (int)BallType.ColorEmpty;
        }
        int randomBall = Random.Range((int)(BallType.NormalBallStart) + 1, ballTypeEnd);
        _BallType = (BallType)randomBall;
    }

    public void SetRandomBall(List<BallType> exBalls)
    {
        if (_BallInfoSP != null && !_BallInfoSP.IsContentNormal())
        {
            _BallType = BallType.None;
            return;
        }

        if (exBalls.Count == 0)
        {
            SetRandomBall();
            return;
        }

        List<int> randomTypes = new List<int>();

        int ballTypeEnd = (int)BallType.NormalBallEnd;
        if (!BallBox.Instance._IsContainEmptyNormal)
        {
            ballTypeEnd = (int)BallType.ColorEmpty;
        }
        for (int i = (int)(BallType.NormalBallStart) + 1; i < ballTypeEnd; ++i)
        {
            for (int j = 0; j < exBalls.Count; ++j)
            {
                if ((int)exBalls[j] == i)
                {
                    break;
                }

                if (j == exBalls.Count - 1)
                {
                    randomTypes.Add(i);
                }
            }
        }

        int randomIdx = Random.Range(0, randomTypes.Count);
        _BallType = (BallType)randomTypes[randomIdx];
    }

    #endregion

    #region opt

    public int _BornRound = 0;

    public bool IsCanExChange(BallInfo other)
    {
        if (other._BallInfoSP != null && _BallInfoSP != null)
        {
            return _BallInfoSP.IsCanExchange(other) & other._BallInfoSP.IsCanExchange(this); ;
        }
        else if (_BallInfoSP != null)
        {
            return _BallInfoSP.IsCanExchange(other);
        }
        else if (other._BallInfoSP != null)
        {
            return other._BallInfoSP.IsCanExchange(this);
        }
        else if (_BallType == BallType.None && other.BallType == BallType.None)
        {
            return false;
        }
        return true;
    }

    public bool ExChangeBall(BallInfo other)
    {
        if (!IsCanExChange(other))
            return false;

        var temp = other.BallType;
        other.BallType = _BallType;
        _BallType = temp;

        var tempRound = other._BornRound;
        other._BornRound = _BornRound;
        _BornRound = tempRound;

        if (_BallInfoSP != null && _BallInfoSP.IsExchangeSpInfo(other))
        {
            var tempSP = other._BallInfoSP;
            other._BallInfoSP = _BallInfoSP;
            _BallInfoSP = tempSP;
            

            var tempSpType = other.BallSPType;
            other.BallSPType = BallSPType;
            BallSPType = tempSpType;

            if (_BallInfoSP != null)
            {
                _BallInfoSP.SetBallInfo(this);
            }
            other._BallInfoSP.SetBallInfo(other);
        }
        else if (other._BallInfoSP != null && other._BallInfoSP.IsExchangeSpInfo(this))
        {
            var tempSP = other._BallInfoSP;
            other._BallInfoSP = _BallInfoSP;
            _BallInfoSP = tempSP;
            _BallInfoSP.SetBallInfo(other);
            
            var tempSpType = other.BallSPType;
            other.BallSPType = BallSPType;
            BallSPType = tempSpType;

            _BallInfoSP.SetBallInfo(this);
            if (other._BallInfoSP != null)
            {
                other._BallInfoSP.SetBallInfo(other);
            }
        }

        return true;
    }

    public void OnRoundEnd()
    {
        if (_BallInfoSP != null)
        {
            _BallInfoSP.OnRoundEnd();
        }
    }
    #endregion

    #region elimit

    public int _ElimitFlag = 0;


    public bool IsCanNormalElimit(BallInfo other)
    {
        if (_BallInfoSP != null && other._BallInfoSP != null)
        {
            if (!_BallInfoSP.IsCanNormalElimit() || !other._BallInfoSP.IsCanNormalElimit())
            {
                return false;
            }
        }
        else if (_BallInfoSP != null)
        {
            if (!_BallInfoSP.IsCanNormalElimit())
                return false;
        }
        else if (other._BallInfoSP != null)
        {
            if (!other._BallInfoSP.IsCanNormalElimit())
                return false;
        }

        return IsEquipNormal(other);
    }

    public bool IsCanNormalElimit()
    {
        if (_BallInfoSP != null)
        {
            return _BallInfoSP.IsCanNormalElimit();
        }

        return true;
    }

    public bool IsCanBeSPElimit(BallInfo other)
    {
        if (_BallInfoSP != null)
        {
            return _BallInfoSP.IsCanBeSPElimit(other);
        }

        return true;
    }

    public void OnNormalElimit()
    {
        if (_BallInfoSP != null)
        {
            _BallInfoSP.OnNormalElimit();
            return;
        }
        Clear();
    }

    public void OnSPElimit()
    {
        if (_BallInfoSP != null)
        {
            _BallInfoSP.OnSPElimit();
            return;
        }

        OnNormalElimit();
    }

    public bool IsCanMove()
    {
        if (_BallInfoSP != null)
        {
            return _BallInfoSP.IsCanMove();
        }

        return true;
    }

    public bool IsCanPass()
    {
        if (_BallInfoSP != null)
        {
            return _BallInfoSP.IsCanPass();
        }

        return true;
    }

    public bool IsCanFall( )
    {
        if (_BallInfoSP != null)
        {
            return _BallInfoSP.IsCanFall();
        }

        return  IsNormalBall();
    }

    public bool IsExplore()
    {
        if (_BallInfoSP != null)
        {
            return _BallInfoSP.IsExplore();
        }

        return false;
    }

    public bool IsRemain()
    {
        if (_BallInfoSP != null)
        {
            return _BallInfoSP.IsRemain();
        }

        return false;
    }

    public void OnExplore()
    {
        if (_BallInfoSP != null)
        {
            _BallInfoSP.OnExplore();
        }
        
    }

    public List<BallInfo> CheckSPElimit()
    {
        if (_BallInfoSP != null)
        {
            return _BallInfoSP.CheckSPElimit();
        }

        return null;
    }

    public List<BallInfo> CheckSPMove(List<BallInfo> checkBalls)
    {
        if (_BallInfoSP != null)
        {
            return _BallInfoSP.CheckSPMove(checkBalls);
        }

        return null;
    }

    public void SpRemove()
    {
        _BallInfoSP = null;
        _BallSPType = BallType.None;
    }
    #endregion
}
