﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallInfoSPTrapStone : BallInfoSPBase
{
    public int ElimitNum = 3;

    public override bool IsCanExchange(BallInfo other)
    {
        return false;
    }

    public override bool IsExchangeSpInfo(BallInfo other)
    {
        return true;
    }

    public override bool IsCanNormalElimit()
    {
        return false;
    }

    public override bool IsContentNormal()
    {
        return false;
    }

    public override bool IsCanFall()
    {
        return false;
    }

    public override bool IsCanMove()
    {
        return false;
    }

    public override bool IsCanPass()
    {
        return false;
    }

    public override bool IsExplore()
    {
        
        return false;
    }

    public override void OnExplore()
    {
        
    }

    public override void OnSPElimit()
    {
        ElimitNum -= BallBox.Instance._OptImpact._DamageToTrap;
        if (ElimitNum < 0)
        {
            _BallInfo.SpRemove();
        }
    }

    public override void SetParam(string[] param)
    {
        if (param.Length > 1)
        {
            ElimitNum = int.Parse(param[1]);
        }
    }
}