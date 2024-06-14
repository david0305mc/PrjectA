using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public partial class Utill
{
    protected static Dictionary<int, string> RegexPatternTable = new Dictionary<int, string>();

    public static void ClearAllPattern()
    {
        RegexPatternTable.Clear();
    }
    public static void AddPattern(int key, string pattern)
    {
        if (RegexPatternTable.ContainsKey(key) == false)
        { RegexPatternTable.Add(key, pattern); }
    }

    public static bool IsMatchPattern(int key, string str)
    {
        return Regex.IsMatch(str, RegexPatternTable[key]);
    }
    public static string Replace(int key, string str, string replace)
    {
        return Regex.Replace(str, RegexPatternTable[key], replace);
    }

    public static string ConvertCommaStringFromInt(int _value)
    {
        string value = string.Format("{0:n0}", _value);

        return value;
    }

    public int ConvertIntFromCommaString(string _str)
    {
        string replaceStr = _str.Replace(",", "");
        int value = int.Parse(replaceStr);

        return value;
    }

}