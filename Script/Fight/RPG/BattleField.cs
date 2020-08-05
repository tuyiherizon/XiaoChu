using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tables;

public class BattleField
{
    #region static

    private static BattleField _Instance;

    public static BattleField Instance
    {
        get
        {
            if (_Instance == null)
                _Instance = new BattleField();

            return _Instance;
        }
    }

    #endregion

    #region 

    private StageInfoRecord _StageRecord;

    private MotionBase _RoleMotion;

    private StageLogic _StageLogic;

    public void InitBattle(StageInfoRecord stageRecord)
    {
        _StageRecord = stageRecord;

        ResourceManager.Instance.LoadPrefab(_StageRecord.FightLogicPath, InitPrefabCallBack, null);
    }

    private void InitPrefabCallBack(string resName, GameObject resGO, Hashtable hashtable)
    {
        _StageLogic = resGO.GetComponent<StageLogic>();
        InitRole();
        StartNextWave();

        LogicManager.Instance.EnterFightFinish();
    }

    private void InitRole()
    {
        _RoleMotion = new MotionBase();
        _RoleMotion._Attack = 10;
        _RoleMotion._Defence = 10;
        _RoleMotion._HP = 1000;
        _RoleMotion._ElementType = ELEMENT_TYPE.NONE;
    }

    private void BattleSucess()
    {

    }

    private void BattleFail()
    {

    }

    #endregion

    #region enemy

    public int _CurWave = -1;

    public List<MotionBase> _Monster;

    private void StartNextWave()
    {
        ++_CurWave;

        if (_StageLogic._Waves.Count <= _CurWave)
        {
            BattleSucess();
            return;
        }

        _Monster = new List<MotionBase>();
        foreach (var monsterID in _StageLogic._Waves[_CurWave].NPCs)
        {
            var monsterMotion = GetMonsterMotion(monsterID);
            if (monsterMotion == null)
            {
                Debug.LogError("Create monster motion error:" + monsterID);
            }
            _Monster.Add(monsterMotion);
        }

        RoundStart();
    }

    public MotionBase GetMonsterMotion(string monsterID)
    {
        MotionBase motionBase = new MotionBase();
        motionBase.InitMonster(TableReader.MonsterBase.GetRecord(monsterID));
        return motionBase;
    }

    #endregion

    #region calculate

    private Dictionary<BallType, int> _RoundDamageBalls = new Dictionary<BallType, int>();
    public const int _DamageOptRound = 3;
    private int _CurOptRound = 1;

    public void RoundStart()
    {

    }

    public void RoundEnd()
    {
        if (_Monster.Count == 0)
        {
            StartNextWave();
            return;
        }

        foreach (var monster in _Monster)
        {
            var result = _RoleMotion.CastDamage(monster._Attack);
            if (result.MotionDie)
            {
                BattleFail();
                break;
            }
        }
    }

    public void BallDamage(Dictionary<BallType, int> elimitBalls)
    {
        ++_CurOptRound;

        foreach (var elimitBall in elimitBalls)
        {
            if (!_RoundDamageBalls.ContainsKey(elimitBall.Key))
            {
                _RoundDamageBalls.Add(elimitBall.Key, 0);
            }

            _RoundDamageBalls[elimitBall.Key] += elimitBall.Value;
        }

        if (_CurOptRound == _DamageOptRound)
        {
            string elimitBallCnt = "";
            int damageBalls = 0;
            foreach (var elimitBall in _RoundDamageBalls)
            {
                elimitBallCnt += " ," + elimitBall.Key.ToString();
                elimitBallCnt += ";" + elimitBall.Value.ToString();
                if (elimitBall.Key > BallType.NormalBallStart && elimitBall.Key < BallType.NormalBallEnd)
                {
                    damageBalls += elimitBall.Value;
                }
                if (_Monster.Count == 0)
                {
                    continue;
                }
                var result = _Monster[0].CastDamage(elimitBall.Value);

                if (result.MotionDie)
                {

                    MotionDie(_Monster[0]);
                }
            }
            Debug.Log("round:" + elimitBallCnt);
            Debug.Log("round damage:" + damageBalls);
            RoundEnd();
            _RoundDamageBalls.Clear();
            _CurOptRound = 0;
        }
    }

    public void MotionDie(MotionBase motion)
    {
        _Monster.Remove(motion);
    }

    #endregion
}
