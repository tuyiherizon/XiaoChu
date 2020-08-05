using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataValue
{
    public static float ConfigIntToFloat(int val)
    {
        var resultVal = new decimal(0.0001) * new decimal(val);
        return (float)resultVal;
    }

    public static float ConfigIntToFloatDex1(int val)
    {
        int dex = Mathf.RoundToInt(val * 0.1f);
        var resultVal = new decimal(0.001) * new decimal(dex);
        return (float)resultVal;
    }

    public static float ConfigIntToPersent(int val)
    {

        var resultVal = new decimal(0.01) * new decimal(val);
        return (float)resultVal;
    }

    public static int ConfigFloatToInt(float val)
    {
        return Mathf.RoundToInt(val * 10000);
    }

    public static int ConfigFloatToPersent(float val)
    {
        float largeVal = val * 100;
        var intVal = Mathf.RoundToInt(largeVal);
        return intVal;
    }

    public static int GetMaxRate()
    {
        return 10000;
    }
    
}
