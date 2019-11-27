using System;
using System.Collections.Generic;

public static class ListExtension
{
    //Code snipet from: http://www.vcskicks.com/code-snippet/shuffle-array.php
    //License can be found at: http://www.vcskicks.com/license.php
    private static Random random = new Random();

    public static void Shuffle<E>(this IList<E> list)
    {
        if (list.Count > 1)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                E tmp = list[i];
                int randomIndex = random.Next(i + 1);

                //Swap elements
                list[i] = list[randomIndex];
                list[randomIndex] = tmp;
            }
        }
    }
}