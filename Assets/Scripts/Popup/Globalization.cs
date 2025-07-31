using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

public class Globalization
{
    public static CultureInfo _culture = CultureInfo.GetCultureInfo("en-US");
    public static float _value = 0f;
    public static float FloatParse(string _str)
    {
        _str = _str.Replace(" ", ".");
        _str = _str.Replace(",", ".");
        _value = float.Parse(_str, _culture);
        return _value;
    }
}