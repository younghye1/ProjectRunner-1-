
using System.Collections.Generic;

public static class ListExtension
{
    // T 는 Template
    public static T Random<T>(this List<T> list)
    {
        int rnd = UnityEngine.Random.Range(0, list.Count);

        return list[rnd];
    }
}