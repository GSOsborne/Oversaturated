using System.Collections.Generic;
using System;

public class CustomUtils
{
    public static List<T> Reshuffle<T>(List<T> list)
    {
        return Reshuffle(list, new Random());
    }

    // Source: https://forum.unity.com/threads/randomize-array-in-c.86871/ (HarvesteR)
    public static List<T> Reshuffle<T>(List<T> list, Random rand)
    {

        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        for (int t = 0; t < list.Count; t++)
        {
            T tmp = list[t];
            int r = rand.Next(t, list.Count);
            list[t] = list[r];
            list[r] = tmp;
        }

        return list;
    }


}
