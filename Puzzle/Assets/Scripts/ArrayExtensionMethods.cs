using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ArrayExtensionMethods
{
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static void Shuffle(this ArrayList array)
    {
        List<object> list = new List<object>();
        list.AddRange(array.ToArray());
        list.Shuffle();
        array.RemoveRange(0, array.Count);
        array.AddRange(list);
    }

    public static void ArrangeAscending(this ArrayList array)
    {
        List<object> list = new List<object>();
        list.AddRange(array.ToArray());
        for (int i = 0; i < list.Count - 1; i++)
        {
            for (int j = i + 1; j < list.Count; j++)
            {
                string num1 = list[i].ToString();
                string num2 = list[j].ToString();
                if (int.Parse(num2) < int.Parse(num1))
                {
                    string temp = num2;
                    num2 = num1;
                    num1 = temp;
                    list[i] = int.Parse(num1);
                    list[j] = int.Parse(num2);
                }
            }
        }
        array.RemoveRange(0, array.Count);
        array.AddRange(list);
    }

    public static void PrintObjects(this ArrayList array)
    {
        for (int i = 0; i < array.Count; i++)
        {
            Debug.Log(array[i]);
        }
    }

    public static string FirstLetterToUpperCaseOrConvertNullToEmptyString(this string s)
    {
        if (string.IsNullOrEmpty(s))
            return string.Empty;

        char[] a = s.ToCharArray();
        a[0] = char.ToUpper(a[0]);
        return new string(a);
    }
}