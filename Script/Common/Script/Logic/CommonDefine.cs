using UnityEngine;
using System.Collections;



public class CommonDefine
{
    public static string GetMigicAttrColor()
    {
        return "<color=#3ba0ff>";
    }

    public static string GetEnableRedStr(int isEnable)
    {
        switch (isEnable)
        {
            case 1:
                return "<color=#00ff00>";
            case 0:
                return "<color=#ff0000>";
        }
        return "<color=#ffffff>";
    }

    public static string GetEnableGrayStr(int isEnable)
    {
        switch (isEnable)
        {
            case 1:
                return "<color=#A8CACAFF>";
            case 0:
                return "<color=#777777>";
        }
        return "<color=#ffffff>";
    }
    
    public static string GetMoneyIcon(MONEYTYPE moneyType)
    {
        switch (moneyType)
        {
            case MONEYTYPE.GOLD:
                return "money/money_gold";
            case MONEYTYPE.DIAMOND:
                return "money/money_diamond";
            default:
                return "money/money_gold";
        }
    }

    /// <summary>
    /// color 转换hex
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public static string ColorToHex(Color color)
    {
        int r = Mathf.RoundToInt(color.r * 255.0f);
        int g = Mathf.RoundToInt(color.g * 255.0f);
        int b = Mathf.RoundToInt(color.b * 255.0f);
        int a = Mathf.RoundToInt(color.a * 255.0f);
        string hex = string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", r, g, b, a);
        return hex;
    }

    /// <summary>
    /// hex转换到color
    /// </summary>
    /// <param name="hex"></param>
    /// <returns></returns>
    public static Color HexToColor(string hex)
    {
        byte br = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte bg = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte bb = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        byte cc = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
        float r = br / 255f;
        float g = bg / 255f;
        float b = bb / 255f;
        float a = cc / 255f;
        return new Color(r, g, b, a);
    }
}

