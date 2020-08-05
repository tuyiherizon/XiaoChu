using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tables;

public class DamageResult
{
    public int DamageValue = 0;
    public bool MotionDie = false;
}

public class MotionBase
{
    #region attr

    public int _Attack;

    public int _Defence;

    public int _HP;

    public ELEMENT_TYPE _ElementType;

    #endregion

    public MonsterBaseRecord _MonsterRecord;

    public void InitMonster(MonsterBaseRecord monster)
    {
        _MonsterRecord = monster;

        _Attack = monster.Attack;
        _Defence = monster.Defence;
        _HP = monster.HP;
        _ElementType = monster.ElementType;
    }

    public DamageResult CastDamage(int damage)
    {
        DamageResult result = new DamageResult();
        result.DamageValue = damage;

        _HP -= damage;
        if (_HP <= 0)
        {
            result.MotionDie = true;
        }

        if (_MonsterRecord != null)
        {
            //Debug.Log("motion cast damage:" + _MonsterRecord.Id + ",value=" + result.DamageValue);
        }
        else
        {
            //Debug.Log("motion cast damage:role," + "value=" + result.DamageValue);
        }
        return result;
    }
}
