using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tables;
using System;

public class ExchangeBalls
{
    public BallInfo FromBall;
    public BallInfo ToBall;

    public List<BallInfo> PathBalls;

    public ExchangeBalls(BallInfo from, BallInfo to)
    {
        FromBall = from;
        ToBall = to;
    }

    public ExchangeBalls(BallInfo from, BallInfo to, List<BallInfo> pathBalls)
    {
        FromBall = from;
        ToBall = to;
        PathBalls = pathBalls;
    }
}

public class EliminateInfo
{
    public BallInfo KeyBall;
    public BallInfo MoveBall;

    public int EliminateCnt;
}

public class OptExtra
{
    public string _ExtraType;
    public BallInfo _OptBall;
}

public class BallBox
{
    #region static

    private static BallBox _Instance;

    public static BallBox Instance
    {
        get
        {
            if (_Instance == null)
                _Instance = new BallBox();

            return _Instance;
        }
    }

    public static bool IsTypeNormal(int type)
    {
        return (BallType)type > BallType.NormalBallStart && (BallType)type < BallType.NormalBallEnd;
    }

    public static bool IsTypeSP(int type)
    {
        return (BallType)type > BallType.SPTrapStart && (BallType)type < BallType.SPLineEnd;
    }
    #endregion


    #region base

    private int _BoxWidth = 6;
    public int BoxWidth
    {
        get
        {
            return _BoxWidth;
        }
    }

    public int _BoxHeight = 7;
    public int BoxHeight
    {
        get
        {
            return _BoxHeight;
        }
    }

    public const int _DefaultPassWidth = 2;
    public const int _DefaultDisapearCnt = 3;
    public BallInfo[][] _BallBoxInfo;
    public bool _IsContainEmptyNormal = false;

    private StageMapRecord _MapRecord;

    public void Init(StageMapRecord mapRecord)
    {
        _MapRecord = mapRecord;
        _BoxWidth = _MapRecord._Width;
        _BoxHeight = _MapRecord._Height;
        _BallBoxInfo = new BallInfo[_BoxWidth][];
        for (int i = 0; i < _BoxWidth; ++i)
        {
            _BallBoxInfo[i] = new BallInfo[_BoxHeight];
            for (int j = 0; j < _BoxHeight; ++j)
            {
                _BallBoxInfo[i][j] = new BallInfo(i,j);
            }
        }

        var weaponType = Type.GetType(WeaponDataPack.Instance.SelectedWeaponItem.Script);
        Debug.Log("OptType:" + WeaponDataPack.Instance.SelectedWeaponItem.Script);
        _OptImpact = Activator.CreateInstance(weaponType) as OptImpactBase;
        _OptImpact.Init();
        _Round = 1;
    }

    public void Refresh()
    {
        for (int j = 0; j < _BallBoxInfo[0].Length; ++j)
        {
            for (int i = 0; i < _BallBoxInfo.Length; ++i)
            {
                _BallBoxInfo[i][j].Clear();
            }
        }
        InitBallInfo();
    }

    public void RefreshNormal()
    {
        List<BallInfo> normalBalls = new List<BallInfo>();
        for (int j = 0; j < _BallBoxInfo[0].Length; ++j)
        {
            for (int i = 0; i < _BallBoxInfo.Length; ++i)
            {
                if (_BallBoxInfo[i][j].IsNormalBall())
                {
                    normalBalls.Add(_BallBoxInfo[i][j]);
                }
            }
        }
        foreach(var normalBall in normalBalls)
        {
            //for (int i = 0; i < _BallBoxInfo.Length; ++i)
            {
                int i = (int)normalBall.Pos.x;
                int j = (int)normalBall.Pos.y;
                List<BallType> exportBall = new List<BallType>();
                if (i > _DefaultDisapearCnt - 2)
                {
                    if (_BallBoxInfo[i - 1][j].IsCanNormalElimit(_BallBoxInfo[i - 2][j]))
                    {
                        exportBall.Add(_BallBoxInfo[i - 1][j].BallType);
                    }
                }

                if (i < _BallBoxInfo.Length - _DefaultDisapearCnt + 1)
                {
                    if (_BallBoxInfo[i + 1][j].IsCanNormalElimit(_BallBoxInfo[i + 2][j]))
                    {
                        exportBall.Add(_BallBoxInfo[i + 1][j].BallType);
                    }
                }

                if (j > _DefaultDisapearCnt - 2)
                {
                    if (_BallBoxInfo[i][j - 1].IsCanNormalElimit(_BallBoxInfo[i][j - 2]))
                    {
                        exportBall.Add(_BallBoxInfo[i][j - 1].BallType);
                    }
                }

                if (j < _BallBoxInfo[0].Length - _DefaultDisapearCnt + 1)
                {
                    if (_BallBoxInfo[i][j + 1].IsCanNormalElimit(_BallBoxInfo[i][j + 2]))
                    {
                        exportBall.Add(_BallBoxInfo[i][j + 1].BallType);
                    }
                }

                if (j < _BallBoxInfo[0].Length - 1 && j > 0)
                {
                    if (_BallBoxInfo[i][j + 1].IsCanNormalElimit(_BallBoxInfo[i][j - 1]))
                    {
                        exportBall.Add(_BallBoxInfo[i][j + 1].BallType);
                    }
                }

                if (i < _BallBoxInfo.Length - 1 && i > 0)
                {
                    if (_BallBoxInfo[i + 1][j].IsCanNormalElimit(_BallBoxInfo[i - 1][j]))
                    {
                        exportBall.Add(_BallBoxInfo[i + 1][j].BallType);
                    }
                }

                if (exportBall.Count > 0)
                {
                    string exBall = "";
                    foreach (var exBallitem in exportBall)
                    {
                        exBall += ((int)exBallitem).ToString();
                    }
                }

                _BallBoxInfo[i][j].SetRandomBall(exportBall);
            }

        }
    }


    private void InitDefault()
    {
        foreach (var mapPos in _MapRecord._MapDefaults)
        {
            string[] mapPosSplit = mapPos.Key.Split(',');
            var ballInfo = GetBallInfo(int.Parse(mapPosSplit[0]),int.Parse(mapPosSplit[1]));
            if (ballInfo != null)
            {
                string spType = mapPos.Value;
                if (!string.IsNullOrEmpty(spType))
                {
                    ballInfo.SetBallInitType(spType);
                }
            }
        }
    }

    public void InitBallInfo()
    {
        BallInfoSPTrapPosion.InitPosion();
        BallInfoSPLineReact.InitReactBalls();

        InitDefault();
        for (int j = 0; j < _BallBoxInfo[0].Length; ++j)
        {
            for (int i = 0; i < _BallBoxInfo.Length; ++i)
            {
                if (!_BallBoxInfo[i][j].IsCanFillNormal())
                    continue;

                List<BallType> exportBall = new List<BallType>();
                if (i > _DefaultDisapearCnt - 2)
                {
                    if (_BallBoxInfo[i - 1][j].IsCanNormalElimit(_BallBoxInfo[i - 2][j]))
                    {
                        exportBall.Add(_BallBoxInfo[i - 1][j].BallType);
                    }
                }

                if (i < _BallBoxInfo.Length - _DefaultDisapearCnt + 1)
                {
                    if (_BallBoxInfo[i + 1][j].IsCanNormalElimit(_BallBoxInfo[i + 2][j]))
                    {
                        exportBall.Add(_BallBoxInfo[i + 1][j].BallType);
                    }
                }

                if (j > _DefaultDisapearCnt - 2)
                {
                    if (_BallBoxInfo[i][j - 1].IsCanNormalElimit(_BallBoxInfo[i][j - 2]))
                    {
                        exportBall.Add(_BallBoxInfo[i][j - 1].BallType);
                    }
                }

                if (j < _BallBoxInfo[0].Length - _DefaultDisapearCnt + 1)
                {
                    if (_BallBoxInfo[i][j + 1].IsCanNormalElimit(_BallBoxInfo[i][j + 2]))
                    {
                        exportBall.Add(_BallBoxInfo[i][j + 1].BallType);
                    }
                }

                if (j < _BallBoxInfo[0].Length - 1 && j > 0)
                {
                    if (_BallBoxInfo[i][j + 1].IsCanNormalElimit(_BallBoxInfo[i][j - 1]))
                    {
                        exportBall.Add(_BallBoxInfo[i][j + 1].BallType);
                    }
                }

                if (i < _BallBoxInfo.Length - 1 && i > 0)
                {
                    if (_BallBoxInfo[i + 1][j].IsCanNormalElimit(_BallBoxInfo[i - 1][j]))
                    {
                        exportBall.Add(_BallBoxInfo[i + 1][j].BallType);
                    }
                }

                if (exportBall.Count > 0)
                {
                    string exBall = "";
                    foreach (var exBallitem in exportBall)
                    {
                        exBall += ((int)exBallitem).ToString();
                    }
                }

                _BallBoxInfo[i][j].SetRandomBall(exportBall);
                _BallBoxInfo[i][j]._BornRound = _Round;
            }

        }
    }

    public BallInfo GetBallInfo(int x, int y)
    {
        if (x < 0 || x >= _BallBoxInfo.Length)
            return null;

        if (y < 0 || y >= _BallBoxInfo[0].Length)
            return null;

        return _BallBoxInfo[x][y];
    }


    #endregion

    #region opt

    public int _Round = 0;
    public int _OptRound = 0;

    public OptImpactBase _OptImpact;

    public void MoveBall(BallInfo ballA, BallInfo ballB)
    {
        ++_Round;
        ++_OptRound;
        ballA.ExChangeBall(ballB);
    }

    public void MoveBack(BallInfo ballA, BallInfo ballB)
    {
        --_Round;
        --_OptRound;
        ballA.ExChangeBall(ballB);
    }

    public void ExchangeBalls(BallInfo ballA, BallInfo ballB)
    {
        if (!ballA.IsCanExChange(ballB))
            return;

        ballA.ExChangeBall(ballB);
    }

    public List<BallInfo> RoundEnd()
    {
        BattleField.Instance.BallDamage(_RoundTotalEliminate);
        _RoundTotalEliminate.Clear();

        var reshowBalls = BallInfoSPTrapPosion.OnPosionRoundEnd();
        return reshowBalls;
    }

    public bool IsCanMoveTo(BallInfo ballSource, BallInfo ballDest)
    {
        if (ballDest.Pos.x < 0 || ballDest.Pos.x >= _BallBoxInfo.Length)
            return false;

        if (ballDest.Pos.y < 0 || ballDest.Pos.y >= _BallBoxInfo[0].Length)
            return false;

        int xdelta = (int)Mathf.Abs(ballDest.Pos.x - ballSource.Pos.x);
        int ydelta = (int)Mathf.Abs(ballDest.Pos.y - ballSource.Pos.y);
        if (xdelta + ydelta == 1)
            return true;

        return false;
    }
    #endregion

    #region elimit

    private Dictionary<BallType, int> _RoundTotalEliminate = new Dictionary<BallType, int>();

    private List<BallInfo> _CurrentNormalEliminateBalls = new List<BallInfo>();
    private List<BallInfo> _CurrentSPEliminateBalls = new List<BallInfo>();
    private List<OptExtra> _CurrentOptExtra = new List<OptExtra>();

    private void AddEliminateBall(BallInfo ballInfo)
    {
        if (!_CurrentNormalEliminateBalls.Contains(ballInfo))
        {
            _CurrentNormalEliminateBalls.Add(ballInfo);
        }

        
    }

    private void AddSPEliminateBall(BallInfo ballInfo)
    {
        if (_CurrentNormalEliminateBalls.Contains(ballInfo))
            return;

        if (!_CurrentSPEliminateBalls.Contains(ballInfo))
        {
            _CurrentSPEliminateBalls.Add(ballInfo);
        }

    }

    private void AddRoundTotalEliminate(BallInfo ballInfo)
    {
        if (ballInfo._BallInfoSP != null)
        {
            if (!_RoundTotalEliminate.ContainsKey(ballInfo.BallSPType))
            {
                _RoundTotalEliminate.Add(ballInfo.BallSPType, 0);
            }
            ++_RoundTotalEliminate[ballInfo.BallSPType];
        }
        if (!_RoundTotalEliminate.ContainsKey(ballInfo.BallType))
        {
            _RoundTotalEliminate.Add(ballInfo.BallType, 0);
        }
        ++_RoundTotalEliminate[ballInfo.BallType];
    }

    private void AddOptExtra(OptExtra optExtra)
    {
        if (!_CurrentOptExtra.Contains(optExtra))
        {
            _CurrentOptExtra.Add(optExtra);
        }
    }

    public List<BallInfo> CurrentElimitnate()
    {
        ++_Round;
        List<BallInfo> checkExplore = new List<BallInfo>();
        foreach (var ballInfo in _CurrentNormalEliminateBalls)
        {
            AddRoundTotalEliminate(ballInfo);
            ballInfo.OnNormalElimit();

            checkExplore.Add(ballInfo);
        }

        foreach (var ballInfo in _CurrentSPEliminateBalls)
        {
            if (ballInfo.IsRemain())
                continue;

            bool isContainNormal = ballInfo.IsContainNormal();

            AddRoundTotalEliminate(ballInfo);
            ballInfo.OnSPElimit();
            
            if (isContainNormal)
            {
                if (!ballInfo.IsContainNormal())
                {
                    checkExplore.Add(ballInfo);
                }
            }
        }

        var exploreBalls = GetExploreBalls(checkExplore);

        foreach (var optEx in _CurrentOptExtra)
        {
            _OptImpact.SetElimitExtra(optEx._ExtraType, optEx._OptBall);
        }

        _CurrentNormalEliminateBalls.Clear();
        _CurrentSPEliminateBalls.Clear();
        _CurrentOptExtra.Clear();

        return exploreBalls;
    }

    public List<BallInfo> AfterElimitnate()
    {
        if (BallInfoSPBombAuto.AutoBombList.Count == 0)
            return null;

        List<BallInfo> ballList = new List<BallInfo>();
        foreach (var autoBomb in BallInfoSPBombAuto.AutoBombList)
        {
            autoBomb.BallInfo._BornRound = autoBomb.BallInfo._BornRound - 1;
            ballList.Add(autoBomb.BallInfo);
        }

        var bombBalls = CheckSpElimit(ballList);
        var exploreBalls = CurrentElimitnate();

        BallBox.AddBallInfos(ballList, bombBalls);
        BallBox.AddBallInfos(ballList, exploreBalls);

        return ballList;
    }

    public ExchangeBalls FindAnyEliminate()
    {
        for (int j = 0; j < _BallBoxInfo[0].Length; ++j)
        {
            for (int i = 0; i < _BallBoxInfo.Length; ++i)
            {

                if (_BallBoxInfo[i][j].IsCanNormalElimit(GetBallInfo(i + 1, j)))
                {
                    var moveBall = GetBallInfo(i + 2, j);
                    if (moveBall != null && moveBall.IsCanMove())
                    {
                        if (_BallBoxInfo[i][j].IsCanNormalElimit(GetBallInfo(i + 3, j)))
                        {
                            return new ExchangeBalls(GetBallInfo(i + 3, j), GetBallInfo(i + 2, j));
                        }

                        if (_BallBoxInfo[i][j].IsCanNormalElimit(GetBallInfo(i + 2, j - 1)))
                        {
                            return new ExchangeBalls(GetBallInfo(i + 2, j - 1), GetBallInfo(i + 2, j));
                        }

                        if (_BallBoxInfo[i][j].IsCanNormalElimit(GetBallInfo(i + 2, j + 1)))
                        {
                            return new ExchangeBalls(GetBallInfo(i + 2, j + 1), GetBallInfo(i + 2, j));
                        }
                    }

                    moveBall = GetBallInfo(i - 1, j);
                    if (moveBall != null && moveBall.IsCanMove())
                    {
                        if (_BallBoxInfo[i][j].IsCanNormalElimit(GetBallInfo(i - 2, j)))
                        {
                            return new ExchangeBalls(GetBallInfo(i - 2, j), GetBallInfo(i - 1, j));
                        }

                        if (_BallBoxInfo[i][j].IsCanNormalElimit(GetBallInfo(i - 1, j - 1)))
                        {
                            return new ExchangeBalls(GetBallInfo(i - 1, j - 1), GetBallInfo(i - 1, j));
                        }

                        if (_BallBoxInfo[i][j].IsCanNormalElimit(GetBallInfo(i - 1, j + 1)))
                        {
                            return new ExchangeBalls(GetBallInfo(i - 1, j + 1), GetBallInfo(i - 1, j));
                        }
                    }
                }

                if (_BallBoxInfo[i][j].IsCanNormalElimit(GetBallInfo(i, j + 1)))
                {
                    var moveBall = GetBallInfo(i, j + 2);
                    if (moveBall != null && moveBall.IsCanMove())
                    {
                        if (_BallBoxInfo[i][j].IsCanNormalElimit(GetBallInfo(i, j + 3)))
                        {
                            return new ExchangeBalls(GetBallInfo(i, j + 3), GetBallInfo(i, j + 2));
                        }

                        if (_BallBoxInfo[i][j].IsCanNormalElimit(GetBallInfo(i - 1, j + 2)))
                        {
                            return new ExchangeBalls(GetBallInfo(i - 1, j + 2), GetBallInfo(i, j + 2));
                        }

                        if (_BallBoxInfo[i][j].IsCanNormalElimit(GetBallInfo(i + 1, j + 2)))
                        {
                            return new ExchangeBalls(GetBallInfo(i + 1, j + 2), GetBallInfo(i, j + 2));
                        }
                    }

                    moveBall = GetBallInfo(i, j - 1);
                    if (moveBall != null && moveBall.IsCanMove())
                    {
                        if (_BallBoxInfo[i][j].IsCanNormalElimit(GetBallInfo(i, j - 2)))
                        {
                            return new ExchangeBalls(GetBallInfo(i, j - 2), GetBallInfo(i, j - 1));
                        }

                        if (_BallBoxInfo[i][j].IsCanNormalElimit(GetBallInfo(i + 1, j - 1)))
                        {
                            return new ExchangeBalls(GetBallInfo(i + 1, j - 1), GetBallInfo(i, j - 1));
                        }

                        if (_BallBoxInfo[i][j].IsCanNormalElimit(GetBallInfo(i - 1, j - 1)))
                        {
                            return new ExchangeBalls(GetBallInfo(i - 1, j - 1), GetBallInfo(i, j - 1));
                        }
                    }
                }

                if (_BallBoxInfo[i][j].IsCanNormalElimit(GetBallInfo(i + 1, j + 1)))
                {
                    var moveBall = GetBallInfo(i + 1, j);
                    if (moveBall != null && moveBall.IsCanMove())
                    {
                        moveBall = GetBallInfo(i + 1, j + 1);
                        if (_BallBoxInfo[i][j].IsCanNormalElimit(GetBallInfo(i + 2, j)) && moveBall != null && moveBall.IsCanMove())
                        {
                            return new ExchangeBalls(GetBallInfo(i + 1, j + 1), GetBallInfo(i + 1, j));
                        }

                        moveBall = GetBallInfo(i, j);
                        if (_BallBoxInfo[i][j].IsCanNormalElimit(GetBallInfo(i + 1, j - 1)) && moveBall != null && moveBall.IsCanMove())
                        {
                            return new ExchangeBalls(GetBallInfo(i, j), GetBallInfo(i + 1, j));
                        }
                    }

                    moveBall = GetBallInfo(i, j + 1);
                    if (moveBall != null && moveBall.IsCanMove())
                    {
                        moveBall = GetBallInfo(i + 1, j + 1);
                        if (_BallBoxInfo[i][j].IsCanNormalElimit(GetBallInfo(i, j + 2)) && moveBall != null && moveBall.IsCanMove())
                        {
                            return new ExchangeBalls(GetBallInfo(i + 1, j + 1), GetBallInfo(i, j + 1));
                        }

                        moveBall = GetBallInfo(i, j);
                        if (_BallBoxInfo[i][j].IsCanNormalElimit(GetBallInfo(i - 1, j + 1)) && moveBall != null && moveBall.IsCanMove())
                        {
                            return new ExchangeBalls(GetBallInfo(i, j), GetBallInfo(i, j + 1));
                        }
                    }
                    
                }

                if (_BallBoxInfo[i][j].IsCanNormalElimit(GetBallInfo(i - 1, j + 1)))
                {

                    var moveBall = GetBallInfo(i - 1, j);
                    if (_BallBoxInfo[i][j].IsCanNormalElimit(GetBallInfo(i - 2, j)) && moveBall != null && moveBall.IsCanMove())
                    {
                        return new ExchangeBalls(GetBallInfo(i - 1, j + 1), GetBallInfo(i - 1, j));
                    }

                    moveBall = GetBallInfo(i, j + 1);
                    if (_BallBoxInfo[i][j].IsCanNormalElimit(GetBallInfo(i, j + 2)) && moveBall != null && moveBall.IsCanMove())
                    {
                        return new ExchangeBalls(GetBallInfo(i - 1, j + 1), GetBallInfo(i, j + 1));
                    }

                    moveBall = GetBallInfo(i, j - 1);
                    if (_BallBoxInfo[i][j].IsCanNormalElimit(GetBallInfo(i - 1, j - 1)) && moveBall != null && moveBall.IsCanMove())
                    {
                        return new ExchangeBalls(GetBallInfo(i, j), GetBallInfo(i, j - 1));
                    }

                    moveBall = GetBallInfo(i + 1, j);
                    if (_BallBoxInfo[i][j].IsCanNormalElimit(GetBallInfo(i + 1, j + 1)) && moveBall != null && moveBall.IsCanMove())
                    {
                        return new ExchangeBalls(GetBallInfo(i, j), GetBallInfo(i + 1, j));
                    }
                }

            }
        }

        return null;
    }

    //public ExchangeBalls FindPowerEliminate()
    //{

    //    return null;
    //    for (int j = 0; j < _BallBoxInfo[0].Length; ++j)
    //    {
    //        for (int i = 0; i < _BallBoxInfo.Length; ++i)
    //        {
    //            if(!_BallBoxInfo[i][j].IsCanMove())
    //                continue;

    //            int n = 1;
    //            Dictionary<BallType, Dictionary<int, List<EliminateInfo>>> nearBallInfos = new Dictionary<BallType, Dictionary<int, List<EliminateInfo>>>();
    //            var elimitInfo = GetEliminateInfo(i-1, j, i-2,j);
    //            if (elimitInfo != null)
    //            {
    //                if (!nearBallInfos.ContainsKey(elimitInfo.KeyBall.BallType))
    //                {
    //                    nearBallInfos.Add(elimitInfo.KeyBall.BallType, new Dictionary<int, List<EliminateInfo>>());
    //                }
    //                if (!nearBallInfos[elimitInfo.KeyBall.BallType].ContainsKey(elimitInfo.EliminateCnt))
    //                {
    //                    nearBallInfos[elimitInfo.KeyBall.BallType].Add(elimitInfo.EliminateCnt, new List<EliminateInfo>());
    //                }
    //                nearBallInfos[elimitInfo.KeyBall.BallType][elimitInfo.EliminateCnt].Add(elimitInfo);
    //            }

    //            elimitInfo = GetEliminateInfo(i + 1, j, i + 2, j);
    //            if (elimitInfo != null)
    //            {
    //                if (!nearBallInfos.ContainsKey(elimitInfo.KeyBall.BallType))
    //                {
    //                    nearBallInfos.Add(elimitInfo.KeyBall.BallType, new Dictionary<int, List<EliminateInfo>>());
    //                }
    //                if (!nearBallInfos[elimitInfo.KeyBall.BallType].ContainsKey(elimitInfo.EliminateCnt))
    //                {
    //                    nearBallInfos[elimitInfo.KeyBall.BallType].Add(elimitInfo.EliminateCnt, new List<EliminateInfo>());
    //                }
    //                nearBallInfos[elimitInfo.KeyBall.BallType][elimitInfo.EliminateCnt].Add(elimitInfo);
    //            }

    //            elimitInfo = GetEliminateInfo(i, j - 1, i, j - 2);
    //            if (elimitInfo != null)
    //            {
    //                if (!nearBallInfos.ContainsKey(elimitInfo.KeyBall.BallType))
    //                {
    //                    nearBallInfos.Add(elimitInfo.KeyBall.BallType, new Dictionary<int, List<EliminateInfo>>());
    //                }
    //                if (!nearBallInfos[elimitInfo.KeyBall.BallType].ContainsKey(elimitInfo.EliminateCnt))
    //                {
    //                    nearBallInfos[elimitInfo.KeyBall.BallType].Add(elimitInfo.EliminateCnt, new List<EliminateInfo>());
    //                }
    //                nearBallInfos[elimitInfo.KeyBall.BallType][elimitInfo.EliminateCnt].Add(elimitInfo);
    //            }

    //            elimitInfo = GetEliminateInfo(i, j + 1, i, j + 2);
    //            if (elimitInfo != null)
    //            {
    //                if (!nearBallInfos.ContainsKey(elimitInfo.KeyBall.BallType))
    //                {
    //                    nearBallInfos.Add(elimitInfo.KeyBall.BallType, new Dictionary<int, List<EliminateInfo>>());
    //                }
    //                if (!nearBallInfos[elimitInfo.KeyBall.BallType].ContainsKey(elimitInfo.EliminateCnt))
    //                {
    //                    nearBallInfos[elimitInfo.KeyBall.BallType].Add(elimitInfo.EliminateCnt, new List<EliminateInfo>());
    //                }
    //                nearBallInfos[elimitInfo.KeyBall.BallType][elimitInfo.EliminateCnt].Add(elimitInfo);
    //            }

    //            EliminateInfo keyEliminateInfo = null;
    //            foreach (var nearBalls in nearBallInfos)
    //            {
    //                if (nearBalls.Value.Count == 2)
    //                {
    //                    if (nearBalls.Value.ContainsKey(2))
    //                    {
    //                        if (nearBalls.Value.ContainsKey(1))
    //                        {
    //                            keyEliminateInfo = new EliminateInfo();
    //                            keyEliminateInfo.KeyBall = _BallBoxInfo[i][j];
    //                            keyEliminateInfo.MoveBall = nearBalls.Value[1][0].KeyBall;
    //                        }
    //                        else
    //                        {
    //                            keyEliminateInfo = new EliminateInfo();
    //                            keyEliminateInfo.KeyBall = _BallBoxInfo[i][j];
    //                            keyEliminateInfo.MoveBall = nearBalls.Value[2][0].KeyBall;
    //                        }
    //                    }
    //                }
    //                else if (nearBalls.Value.Count > 2)
    //                {
    //                    if (nearBalls.Value.ContainsKey(1) && nearBalls.Value[1].Count == 1)
    //                    {
    //                        keyEliminateInfo = new EliminateInfo();
    //                        keyEliminateInfo.KeyBall = _BallBoxInfo[i][j];
    //                        keyEliminateInfo.MoveBall = nearBalls.Value[1][0].KeyBall;
    //                    }
    //                    else if (!nearBalls.Value.ContainsKey(2))
    //                    {
    //                        if (nearBalls.Value.ContainsKey(1) && nearBalls.Value[1].Count == 4)
    //                        {
    //                            keyEliminateInfo = new EliminateInfo();
    //                            keyEliminateInfo.KeyBall = _BallBoxInfo[i][j];
    //                            keyEliminateInfo.MoveBall = nearBalls.Value[1][0].KeyBall;
    //                        }
    //                        else if (nearBalls.Value.ContainsKey(1) && nearBalls.Value[1].Count == 3)
    //                        {
    //                            bool isAnyEqual1 = true;
    //                            if ((int)nearBalls.Value[1][0].KeyBall.Pos.x != (int)nearBalls.Value[1][1].KeyBall.Pos.x
    //                                && (int)nearBalls.Value[1][0].KeyBall.Pos.y != (int)nearBalls.Value[1][1].KeyBall.Pos.y)
    //                                isAnyEqual1 = false;

    //                            bool isAnyEqual2 = true;
    //                            if ((int)nearBalls.Value[1][0].KeyBall.Pos.x != (int)nearBalls.Value[1][2].KeyBall.Pos.x
    //                                && (int)nearBalls.Value[1][0].KeyBall.Pos.y != (int)nearBalls.Value[1][2].KeyBall.Pos.y)
    //                                isAnyEqual2 = false;

    //                            if (!isAnyEqual1 && !isAnyEqual2)
    //                            {
    //                                keyEliminateInfo = new EliminateInfo();
    //                                keyEliminateInfo.KeyBall = _BallBoxInfo[i][j];
    //                                keyEliminateInfo.MoveBall = nearBalls.Value[1][0].KeyBall;
    //                            }
    //                            else if (!isAnyEqual1 && !isAnyEqual2)
    //                            {
    //                                keyEliminateInfo = new EliminateInfo();
    //                                keyEliminateInfo.KeyBall = _BallBoxInfo[i][j];
    //                                keyEliminateInfo.MoveBall = nearBalls.Value[1][0].KeyBall;
    //                            }
    //                        }
    //                    }
    //                    else if (nearBalls.Value.ContainsKey(2) && nearBalls.Value[2].Count == 1)
    //                    {
    //                        foreach (var nearBall1 in nearBalls.Value[1])
    //                        {
    //                            if ((int)nearBall1.KeyBall.Pos.x != (int)nearBalls.Value[2][0].KeyBall.Pos.x
    //                                && (int)nearBall1.KeyBall.Pos.y != (int)nearBalls.Value[2][0].KeyBall.Pos.y)
    //                            {
    //                                keyEliminateInfo = new EliminateInfo();
    //                                keyEliminateInfo.KeyBall = _BallBoxInfo[i][j];
    //                                keyEliminateInfo.MoveBall = nearBall1.KeyBall;
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}

    public ExchangeBalls FindPowerEliminate()
    {
        EliminateInfo exangeBall = new EliminateInfo();
        for (int j = 0; j < _BallBoxInfo[0].Length; ++j)
        {
            for (int i = 0; i < _BallBoxInfo.Length; ++i)
            {
                var curBall = GetBallInfo(i,j);
                var nextBall = GetBallInfo(i+1,j);
                if (nextBall == null)
                    continue;

                if (!curBall.IsCanMove() || !nextBall.IsCanMove())
                    continue;

                ExchangeBalls(curBall, nextBall);

                var elimitBallInfos = TestNormalEliminate(new List<BallInfo>() { curBall, nextBall });
                var spElimitsMove = TestSpMove(new List<BallInfo>() { curBall, nextBall });
                var spElimitsElimit = TestSpElimit(elimitBallInfos);
                int checkCnt = elimitBallInfos.Count;
                foreach (var spElimit in spElimitsMove)
                {
                    if (!elimitBallInfos.Contains(spElimit))
                    {
                        ++checkCnt;
                    }
                }
                foreach (var spElimit in spElimitsElimit)
                {
                    if (!elimitBallInfos.Contains(spElimit)
                        && !spElimitsMove.Contains(spElimit))
                    {
                        ++checkCnt;
                    }
                }

                if (checkCnt > exangeBall.EliminateCnt)
                {
                    exangeBall.KeyBall = curBall;
                    exangeBall.MoveBall = nextBall;
                    exangeBall.EliminateCnt = checkCnt;
                }

                ExchangeBalls(curBall, nextBall);

                nextBall = GetBallInfo(i, j + 1);
                if (nextBall == null)
                    continue;
                ExchangeBalls(curBall, nextBall);

                elimitBallInfos = TestNormalEliminate(new List<BallInfo>() { curBall, nextBall });
                spElimitsMove = TestSpMove(new List<BallInfo>() { curBall, nextBall });
                spElimitsElimit = TestSpElimit(elimitBallInfos);
                checkCnt = elimitBallInfos.Count;
                foreach (var spElimit in spElimitsMove)
                {
                    if (!elimitBallInfos.Contains(spElimit))
                    {
                        ++checkCnt;
                    }
                }
                foreach (var spElimit in spElimitsElimit)
                {
                    if (!elimitBallInfos.Contains(spElimit)
                        && !spElimitsMove.Contains(spElimit))
                    {
                        ++checkCnt;
                    }
                }
                if (checkCnt > exangeBall.EliminateCnt)
                {
                    exangeBall.KeyBall = curBall;
                    exangeBall.MoveBall = nextBall;
                    exangeBall.EliminateCnt = checkCnt;
                }
                ExchangeBalls(curBall, nextBall);
            }
        }
        if (exangeBall.KeyBall == null)
            return null;

        return new ExchangeBalls(exangeBall.KeyBall, exangeBall.MoveBall);
    }

    private EliminateInfo GetEliminateInfo(int posX, int posY, int posX2, int posY2)
    {
        var bombBall = GetBallInfo(posX, posY);
        if (bombBall != null && bombBall.IsCanNormalElimit() && bombBall.IsCanMove())
        {
            EliminateInfo elimitInfo = new EliminateInfo();
            elimitInfo.KeyBall = bombBall;
            elimitInfo.EliminateCnt = 1;

            var bombBallEx = GetBallInfo(posX2, posY2);
            if (bombBallEx != null
                && bombBallEx.IsCanNormalElimit(bombBall)
                && bombBallEx.IsCanMove())
            {
                elimitInfo.EliminateCnt = 2;
            }

            return elimitInfo;
        }

        return null;
    }

    public List<BallInfo> TestNormalEliminate(List<BallInfo> checkBalls)
    {
        List<BallInfo> elimitBalls = new List<BallInfo>();
        {
            foreach (var checkBall in checkBalls)
            {
                BallInfo curBall = checkBall;

                //if (curBall._BornRound == _Round)
                //    continue;

                var clumnElimit = CheckClumnElimit((int)checkBall.Pos.x, (int)checkBall.Pos.y);
                var rawElimit = CheckRawElimit((int)checkBall.Pos.x, (int)checkBall.Pos.y);
                int elimitNum = 0;
                if (clumnElimit != null)
                {
                    elimitNum += clumnElimit.Count;
                    elimitBalls.AddRange(clumnElimit);
                }
                if (rawElimit != null)
                {
                    elimitNum += rawElimit.Count;
                    elimitBalls.AddRange(rawElimit);
                }
            }
        }

        return elimitBalls;
    }

    public List<BallInfo> CheckNormalEliminate(List<BallInfo> checkBalls)
    {
        List<BallInfo> elimitBalls = new List<BallInfo>();
        {
            foreach(var checkBall in checkBalls)
            {
                BallInfo curBall = checkBall;

                if (curBall._BornRound == _Round)
                    continue;

                var clumnElimit = CheckClumnElimit((int)checkBall.Pos.x, (int)checkBall.Pos.y);
                var rawElimit = CheckRawElimit((int)checkBall.Pos.x, (int)checkBall.Pos.y);
                int elimitNum = 0;
                int clumnNum = 0;
                int rowNum = 0;
                if (clumnElimit != null)
                {
                    clumnNum = clumnElimit.Count;
                    elimitNum += clumnNum;
                    elimitBalls.AddRange(clumnElimit);

                    foreach (var elimitBall in clumnElimit)
                    {
                        //elimitBall.OnNormalElimit();
                        AddEliminateBall(elimitBall);
                    }
                }
                if (rawElimit != null)
                {
                    rowNum = rawElimit.Count;
                    elimitNum += rowNum;
                    elimitBalls.AddRange(rawElimit);

                    foreach (var elimitBall in rawElimit)
                    {
                        //elimitBall.OnNormalElimit();
                        AddEliminateBall(elimitBall);
                    }
                }

                if (elimitNum > _DefaultDisapearCnt)
                {
                    OptExtra optExtra = new OptExtra();
                    optExtra._ExtraType = _OptImpact.GetExtraSpType(clumnNum, rowNum, curBall);
                    optExtra._OptBall = curBall;
                    //_OptImpact.ElimitExtra(elimitNum, curBall);
                    AddOptExtra(optExtra);
                }
            }
        }

        return elimitBalls;
    }

    public List<BallInfo> CheckClumnElimit(int i, int j)
    {
        BallInfo curBall = GetBallInfo(i, j);
        BallInfo nextClumnBall = GetBallInfo(i + 1, j);
        int nextCntRight = 1;
        while (nextClumnBall != null)
        {
            if (!_CurrentNormalEliminateBalls.Contains(nextClumnBall) && curBall.IsCanNormalElimit(nextClumnBall))
            {
                ++nextCntRight;
                nextClumnBall = GetBallInfo(i + nextCntRight, j);
            }
            else
            {
                break;
            }
        }
        nextClumnBall = GetBallInfo(i - 1, j);
        int nextCntLeft = 1;
        while (nextClumnBall != null)
        {
            if (!_CurrentNormalEliminateBalls.Contains(nextClumnBall) && curBall.IsCanNormalElimit(nextClumnBall))
            {
                ++nextCntLeft;
                nextClumnBall = GetBallInfo(i - nextCntLeft, j);
            }
            else
            {
                break;
            }
        }
        if (nextCntRight + nextCntLeft - 1 >= _DefaultDisapearCnt)
        {
            List<BallInfo> elimitClumn = new List<BallInfo>();
            for (int k = 0; k < nextCntRight; ++k)
            {
                var elimitBall = GetBallInfo(i + k, j);
                if (!elimitClumn.Contains(elimitBall))
                {
                    elimitClumn.Add(elimitBall);
                }
            }
            for (int k = 0; k < nextCntLeft; ++k)
            {
                var elimitBall = GetBallInfo(i - k, j);
                if (!elimitClumn.Contains(elimitBall))
                {
                    elimitClumn.Add(elimitBall);
                }
            }
            return elimitClumn;
        }

        return null;
    }

    public List<BallInfo> CheckRawElimit(int i, int j)
    {
        BallInfo curBall = GetBallInfo(i, j);
        BallInfo nextRawBall = GetBallInfo(i, j + 1);
        var nextCntUp = 1;
        while (nextRawBall != null)
        {
            if (!_CurrentNormalEliminateBalls.Contains(nextRawBall) && curBall.IsCanNormalElimit(nextRawBall))
            {
                ++nextCntUp;
                nextRawBall = GetBallInfo(i, j + nextCntUp);
            }
            else
            {
                break;
            }
        }
        nextRawBall = GetBallInfo(i, j - 1);
        var nextCntDown = 1;
        while (nextRawBall != null)
        {
            if (!_CurrentNormalEliminateBalls.Contains(nextRawBall) && curBall.IsCanNormalElimit(nextRawBall))
            {
                ++nextCntDown;
                nextRawBall = GetBallInfo(i, j - nextCntDown);
            }
            else
            {
                break;
            }
        }
        if (nextCntUp  + nextCntDown - 1 >= _DefaultDisapearCnt)
        {
            List<BallInfo> elimitRaw = new List<BallInfo>();
            for (int k = 0; k < nextCntUp; ++k)
            {
                var elimitBall = GetBallInfo(i, j + k);
                if (!elimitRaw.Contains(elimitBall))
                {
                    elimitRaw.Add(elimitBall);
                }
            }
            for (int k = 0; k < nextCntDown; ++k)
            {
                var elimitBall = GetBallInfo(i, j - k);
                if (!elimitRaw.Contains(elimitBall))
                {
                    elimitRaw.Add(elimitBall);
                }
            }
            return elimitRaw;
        }

        return null;
    }

    public List<BallInfo> CheckSpElimit(List<BallInfo> checkBalls)
    {
        List<BallInfo> spElimit = new List<BallInfo>();

        CheckSpElimitCircle(checkBalls, ref spElimit);

        var reactBalls = BallInfoSPLineReact.GetReactBalls();
        if (reactBalls != null)
        {
            CheckSpElimitCircle(reactBalls, ref spElimit);

            foreach (var subElimit in reactBalls)
            {
                //subElimit.OnSPElimit();
                AddSPEliminateBall(subElimit);
            }
        }

        foreach (var subElimit in spElimit)
        {
            //subElimit.OnSPElimit();
            AddSPEliminateBall(subElimit);
        }

        return spElimit;
    }

    public List<BallInfo> CheckSpMove(List<BallInfo> checkBalls)
    {
        List<BallInfo> spElimit = new List<BallInfo>();

        CheckSpElimitCircle(checkBalls, ref spElimit, true);

        foreach (var subElimit in spElimit)
        {
            //subElimit.OnSPElimit();
            AddSPEliminateBall(subElimit);
        }

        return spElimit;
    }

    public List<BallInfo> TestSpMove(List<BallInfo> checkBalls)
    {
        List<BallInfo> spElimit = new List<BallInfo>();

        CheckSpElimitCircle(checkBalls, ref spElimit, true, true);

        return spElimit;
    }

    public List<BallInfo> TestSpElimit(List<BallInfo> checkBalls)
    {
        List<BallInfo> spElimit = new List<BallInfo>();

        CheckSpElimitCircle(checkBalls, ref spElimit, false, true);

        return spElimit;
    }

    public void CheckSpElimitCircle(List<BallInfo> checkBalls, ref List<BallInfo> spElimitBalls, bool isCheckMove = false, bool testMode = false)
    {
        foreach (var elimitBall in checkBalls)
        {

            List<BallInfo> subElimitList = new List<BallInfo>();
            List<BallInfo> spElimits = null;
            if (!isCheckMove)
            {
                spElimits = elimitBall.CheckSPElimit();
            }
            else
            {
                spElimits = elimitBall.CheckSPMove(checkBalls);
            }
            if (spElimits != null && spElimits.Count > 0)
            {
                foreach (var subElimit in spElimits)
                {
                    if (!testMode)
                    {
                        if (subElimit._BornRound == _Round)
                            continue;
                    }

                    if (!spElimitBalls.Contains(subElimit))
                    {
                        spElimitBalls.Add(subElimit);
                        subElimitList.Add(subElimit);
                    }
                }

                CheckSpElimitCircle(subElimitList, ref spElimitBalls);
            }
        }
    }

    public List<BallInfo> GetExploreBalls(List<BallInfo> elimitBalls)
    {
        List<BallInfo> exploreBalls = new List<BallInfo>();
        foreach (var elimitBall in elimitBalls)
        {
            BallInfo exploreBall = GetBallInfo((int)elimitBall.Pos.x, (int)elimitBall.Pos.y);
            if (!_CurrentNormalEliminateBalls.Contains(exploreBall) && !_CurrentSPEliminateBalls.Contains(exploreBall))
            {
                if (exploreBall != null && exploreBall.IsExplore())
                {
                    exploreBalls.Add(exploreBall);
                }
            }

            exploreBall = GetBallInfo((int)elimitBall.Pos.x + 1, (int)elimitBall.Pos.y);
            if (!_CurrentNormalEliminateBalls.Contains(exploreBall) && !_CurrentSPEliminateBalls.Contains(exploreBall))
            {
                if (exploreBall != null && exploreBall.IsExplore())
                {
                    exploreBalls.Add(exploreBall);
                }
            }

            exploreBall = GetBallInfo((int)elimitBall.Pos.x - 1, (int)elimitBall.Pos.y);
            if (!_CurrentNormalEliminateBalls.Contains(exploreBall) && !_CurrentSPEliminateBalls.Contains(exploreBall))
            {
                if (exploreBall != null && exploreBall.IsExplore())
                {
                    exploreBalls.Add(exploreBall);
                }
            }

            exploreBall = GetBallInfo((int)elimitBall.Pos.x, (int)elimitBall.Pos.y + 1);
            if (!_CurrentNormalEliminateBalls.Contains(exploreBall) && !_CurrentSPEliminateBalls.Contains(exploreBall))
            {
                if (exploreBall != null && exploreBall.IsExplore())
                {
                    exploreBalls.Add(exploreBall);
                }
            }

            exploreBall = GetBallInfo((int)elimitBall.Pos.x, (int)elimitBall.Pos.y - 1);
            if (!_CurrentNormalEliminateBalls.Contains(exploreBall) && !_CurrentSPEliminateBalls.Contains(exploreBall))
            {
                if (exploreBall != null && exploreBall.IsExplore())
                {
                    exploreBalls.Add(exploreBall);
                }
            }
        }

        foreach(var exploreBall in exploreBalls)
        {
            exploreBall.OnExplore();
        }

        return exploreBalls;
    }

    #endregion

    #region 


    //public BallInfo FindFallBall(int x, int y)
    //{
    //    for (int j = y; j < _BallBoxInfo[0].Length; ++j)
    //    {
    //        BallInfo nextBall = GetBallInfo(x, y + 1);
    //        if (nextBall != null && nextBall.IsNormalBall())
    //        {
    //            return nextBall;
    //        }
    //    }
    //    return null;
    //}

    private Dictionary<int, int> _FillPath = new Dictionary<int, int>();
    private List<BallInfo> _NoPassBall = new List<BallInfo>();

    public void ClearFillPath()
    {
        _FillPath.Clear();
        _NoPassBall.Clear();
    }

    public bool IsBallCanPath(BallInfo ballInfo)
    {
        if (ballInfo == null)
            return false;

        if (!ballInfo.IsCanPass())
        {
            return false;
        }

        return true;
    }

    public BallInfo FindFallBall(int x, int y, ref List<BallInfo> pathBalls)
    {
        var curBall = GetBallInfo(x, y);
        if (y >= _BoxHeight-1)
        {
            var fillBall = FindFillPath(x, y);
            pathBalls.Add(fillBall);
            return fillBall;
        }

        BallInfo pathBall = null;
        for (int i = 0; i < _DefaultPassWidth; ++i)
        {
            var nextBall = GetBallInfo(x + i, y + 1);
            if (nextBall!= null && nextBall.IsCanFall())
            {
                pathBalls.Add(nextBall);
                return nextBall;
            }
            else if (nextBall != null && IsBallCanPath(nextBall))
            {
                pathBall = FindFallBall(x + i, y + 1, ref pathBalls);
                if (pathBall != null)
                {
                    pathBalls.Add(nextBall);
                    return pathBall;
                }
            }

            if (i != 0)
            {
                var nextBall2 = GetBallInfo(x - i, y + 1);
                if (nextBall2 != null &&nextBall2.IsCanFall())
                {
                    pathBalls.Add(nextBall2);
                    return nextBall2;
                }
                else if (nextBall2 != null && IsBallCanPath(nextBall2))
                {
                    pathBall = FindFallBall(x - i, y + 1, ref pathBalls);
                    if (pathBall != null)
                    {
                        pathBalls.Add(nextBall2);
                        return pathBall;
                    }
                }
            }
        }

        return null;
    }

    public BallInfo FindFillPath(int x, int y)
    {
        if (!_FillPath.ContainsKey(x))
        {
            _FillPath.Add(x, _BoxHeight - 1);
        }
        ++_FillPath[x];

        var randomFall = new BallInfo(x, _FillPath[x]);
        randomFall.SetRandomBall();
        return randomFall;
    }

    public List<ExchangeBalls> ElimitnateFall()
    {
        ClearFillPath();
        List<ExchangeBalls> exchangeList = new List<ExchangeBalls>();
        for (int j = 0; j < _BallBoxInfo[0].Length; ++j)
        {
            for (int i = 0; i < _BallBoxInfo.Length; ++i)
            {
                BallInfo curBall = GetBallInfo(i, j);
                if (curBall.IsCanFillNormal())
                {
                    List<BallInfo> pathBalls = new List<BallInfo>();
                    var fallBall = FindFallBall((int)curBall.Pos.x, (int)curBall.Pos.y, ref pathBalls);
                    if (fallBall == null)
                        continue;

                    pathBalls.Add(curBall);
                    ExchangeBalls(curBall, fallBall);
                    exchangeList.Add(new ExchangeBalls(fallBall, curBall, pathBalls));
                    //Debug.Log("curPos:" + curBall.Pos + ", tarPos:" + fallBall.Pos + ",type:" + curBall.BallType + ",path:" + pathBalls.Count);
                }
            }
        }

        return exchangeList;
    }

    public List<ExchangeBalls> FillEmpty()
    {
        ClearFillPath();
        List<ExchangeBalls> exchangeList = new List<ExchangeBalls>();
        for (int i = 0; i < _BallBoxInfo.Length; ++i)
        {
            for (int j = 0; j < _BallBoxInfo[0].Length; ++j)
            {

                BallInfo curBall = GetBallInfo(i, j);
                if (curBall.IsCanFillNormal())
                {
                    var ballInfo = FindFillPath(i, j);
                    ballInfo.SetRandomBall();
                    ExchangeBalls(curBall, ballInfo);
                    exchangeList.Add(new ExchangeBalls(ballInfo, curBall));
                }
            }
        }
        return exchangeList;
    }

    #endregion

    #region static

    public static List<BallInfo> AddBallInfos(List<BallInfo> listA, List<BallInfo> listB)
    {
        if (listA == null)
        {
            listA = new List<BallInfo>();
        }
        if (listB != null)
        {
            foreach (var ballInfo in listB)
            {
                if (!listA.Contains(ballInfo))
                {
                    listA.Add(ballInfo);
                }
            }
        }

        return listA;
    }

    #endregion
}
