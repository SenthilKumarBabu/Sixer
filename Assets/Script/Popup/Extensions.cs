using CodeStage.AntiCheat.ObscuredTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public static class Extensions
{
    const int thousand = 1000;
    const int lakh = 100 * thousand;
    const int crore = 100 * lakh;

    /// <summary>Helper method for debugging of List<T> content. Using this is not performant.</summary>
    /// <remarks>Should only be used for debugging as necessary.</remarks>
    /// <param name="data">Any List<T> where T implements .ToString().</param>
    /// <returns>A comma-separated string containing each value's ToString().</returns>
    public static string ToStringFull<T>(this List<T> data)
    {
        if (data == null) return "null";

        string[] sb = new string[data.Count];
        for (int i = 0; i < data.Count; i++)
        {
            object o = data[i];
            sb[i] = (o != null) ? o.ToString() : "null";
        }

        return string.Join(", ", sb);
    }

    /// <summary>Helper method for debugging of object[] content. Using this is not performant.</summary>
    /// <remarks>Should only be used for debugging as necessary.</remarks>
    /// <param name="data">Any object[].</param>
    /// <returns>A comma-separated string containing each value's ToString().</returns>
    public static string ToStringFull(this object[] data)
    {
        if (data == null) return "null";

        string[] sb = new string[data.Length];
        for (int i = 0; i < data.Length; i++)
        {
            object o = data[i];
            sb[i] = (o != null) ? o.ToString() : "null";
        }

        return string.Join(", ", sb);
    }
    public static float GetPreferredWidth(this Text text, float offset, float minWidth)
    {
        float result = text.preferredWidth + offset;
        if (result < minWidth)
        {
            return minWidth;
        }
        return result;
    }

    /// <summary>
    /// Shuffles a generic list. Modifies the original list
    /// </summary>
    /// <typeparam name="T">The type of the items in the list</typeparam>
    /// <param name="input">The list to shuffle</param>
    public static void ShuffleList<T>(this List<T> input)
    {
        for (int i = input.Count - 1; i > 0; i--)
        {
            int rnd = UnityEngine.Random.Range(0, i);
            T temp = input[i];
            input[i] = input[rnd];
            input[rnd] = temp;
        }
    }
    /// <summary>
    /// Clears the array with the default value of the type. Modifies the original list
    /// </summary>
    /// <typeparam name="T">The type of the items in the array</typeparam>
    /// <param name="array">The array to clear</param>
    public static void ClearArray<T>(this T[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = default(T);
        }
    }

    public static T GetComponentInParent<T>(this GameObject go, bool includeInActiveObjects) where T : UnityEngine.Component
    {
        if (!includeInActiveObjects)
            return go.transform.GetComponentInParent<T>();
        var rootTransform = go.transform.root;
        var currentTransform = go.transform;
        while (currentTransform != rootTransform)
        {
            T component = currentTransform.GetComponent<T>();
            if (component.GetInstanceID() != 0)
                return component;
            currentTransform = currentTransform.parent;
        }
        if (currentTransform == null)
            return default(T);
        return currentTransform.GetComponent<T>();
    }
    /// <summary>
    /// Change alpha of an image
    /// </summary>
    /// <param name="image">The image whose alpha need to be changed</param>
    /// <param name="alpha">Aplha value</param>
    public static void Fade(this Image image, float alpha)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
    }

    public static string GetNumberShorthand(this int number)
    {
        string result;
        float addendum;
        if (number >= crore)
        {
            addendum = number * 10 / crore / 10.0f;
            result = string.Format(addendum % 1 == 0 ? "{0:F0}" : "{0:F1}", addendum) + "C";
        }
        else if (number >= lakh)
        {
            addendum = number * 10 / lakh / 10.0f;
            result = string.Format(addendum % 1 == 0 ? "{0:F0}" : "{0:F1}", addendum) + "L";
        }
        else if (number >= thousand)
        {
            addendum = number * 10 / thousand / 10.0f;
            result = string.Format(addendum % 1 == 0 ? "{0:F0}" : "{0:F1}", addendum) + "K";
        }
        else
        {
            result = number.ToString();
        }
        return result;
    }
    
    public static string TimeSpanToString(this TimeSpan unbiasedRemaining)
    {
        string unbiasedFormatted;
        if (unbiasedRemaining.Hours > 0)
        {
            if (unbiasedRemaining.Days > 0)
            {
                unbiasedFormatted = string.Format("{0:D2}{1:D2}{2:D2}", unbiasedRemaining.Days + "d:", unbiasedRemaining.Hours + "h:", unbiasedRemaining.Minutes + "m");
            }
            else
            {
                string hour;
                if (unbiasedRemaining.Hours < 10)
                {
                    hour = "0" + unbiasedRemaining.Hours.ToString();
                }
                else
                {
                    hour = unbiasedRemaining.Hours.ToString();
                }
                string sec1;
                if (unbiasedRemaining.Seconds < 10)
                {
                    sec1 = "0" + unbiasedRemaining.Seconds.ToString();
                }
                else
                {
                    sec1 = unbiasedRemaining.Seconds.ToString();
                }
                unbiasedFormatted = string.Format("{0:D2}{1:D2}{2:D2}", hour + "h:", unbiasedRemaining.Minutes + "m:", sec1 + "s");
            }
        }
        else
        {
            string min;
            if (unbiasedRemaining.Minutes < 10)
            {
                min = "0" + unbiasedRemaining.Minutes.ToString();
            }
            else
            {
                min = unbiasedRemaining.Minutes.ToString();
            }
            string sec;
            if (unbiasedRemaining.Seconds < 10)
            {
                sec = "0" + unbiasedRemaining.Seconds.ToString();
            }
            else
            {
                sec = unbiasedRemaining.Seconds.ToString();
            }

            unbiasedFormatted = string.Format("{0:D2}{1:D2}{2:D2}", "", min + "m:", sec + "s");
        }
        return unbiasedFormatted;
    }

    public static string NumberSystem(int number)
    {
        string str = number.ToString();
        return NumberSystem(str);
    }
    public static string NumberSystem(string number)
    {
        string str = number.ToString();
        string finalStr = str;
        if (str.Length > 3)
        {
            string last3Digits = str.Substring(str.Length - 3, 3);
            string remainingDigits = str.Substring(0, str.Length - 3);
            finalStr = "," + last3Digits;

            string last2Digits;
            while (remainingDigits.Length > 2)
            {
                last2Digits = remainingDigits.Substring(remainingDigits.Length - 2, 2);
                remainingDigits = remainingDigits.Substring(0, remainingDigits.Length - 2);
                finalStr = "," + last2Digits + finalStr;
            }
            finalStr = remainingDigits + finalStr;
        }
        return finalStr;
    }


    public static IEnumerable<T> GetValues<T>()
    {
        return Enum.GetValues(typeof(T)).Cast<T>();
    }

    public static IDictionary<string, object> ToDictionary(this object source)
    {
        return source.ToDictionary<object>();
    }

    public static IDictionary<string, T> ToDictionary<T>(this object source)
    {
        if (source == null) ThrowExceptionWhenSourceArgumentIsNull();

        var dictionary = new Dictionary<string, T>();
        foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(source))
        {
            object value = property.GetValue(source);
            if (IsOfType<T>(value))
            {
                dictionary.Add(property.Name, (T)value);
            }
        }
        return dictionary;
    }

    private static bool IsOfType<T>(object value)
    {
        return value is T;
    }

    private static void ThrowExceptionWhenSourceArgumentIsNull()
    {
        throw new NullReferenceException("Unable to convert anonymous object to a dictionary. The source anonymous object is null.");
    }

    public static Toggle GetActive(this ToggleGroup aGroup)
    {
        return aGroup.ActiveToggles().FirstOrDefault();
    }

    public static void SetIsOnAndNotify(this Toggle toggle, bool value)
    {
        toggle.SetIsOnWithoutNotify(value);
        toggle.onValueChanged?.Invoke(value);
    }
}

public delegate void VoidDelgate();
public delegate void IntDelgate(int value);
public delegate void ObscuredIntDelgate(ObscuredInt value);
public delegate void BoolDelegate(bool status);
public delegate void ObscuredBoolDelegate(ObscuredBool status);
[System.Serializable]
public class BoolIntArrayEvent : UnityEvent<bool, int[]>
{

}

public static class ListExtensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        int count = list.Count;
        System.Random random = new System.Random();
        while (count > 1)
        {
            count--;
            int k = random.Next(count + 1);
            T value = list[k];
            list[k] = list[count];
            list[count] = value;
        }
    }
}

public static class IEnumerableExtensions
{
    public static int[] FindAllIndexof<T>(this IEnumerable<T> values, T val)
    {
        return values.Select((b, i) => object.Equals(b, val) ? i : -1).Where(i => i != -1).ToArray();
    }
}