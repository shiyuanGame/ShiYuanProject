using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellSort
{
    private ShellSort() { }
    public static void Sort<T>(T[] data) where T : System.IComparable<T>
    {
        int h = data.Length / 2;
        while (h >= 1)
        {
            for (var start = 0; start < h; start++)
            {
                //堆 data[ start,start+h,start+2h ,start+3h...] 进行插入排序
                for (var i = start + h; i < data.Length; i += h)
                {
                    T temp = data[i];
                    int j;
                    for (j = i; j - h > 0; j -= h)
                    {
                        if (data[j - h].CompareTo(temp) > 0)
                        {
                            data[j] = data[j - h];
                        }
                    }
                    data[j] = temp;
                }
            }
            h /= 2;
        }
    }
    public static void Sort2<T>(T[] data) where T : System.IComparable<T>
    {
        int h = data.Length / 2;
        while (h >= 1)
        {
            for (var i = h; i < data.Length; i++)
            {
                T temp = data[i];
                int j;
                for (j = i; j - h > 0; j -= h)
                {
                    if (data[j - h].CompareTo(temp) > 0)
                    {
                        data[j] = data[j - h];
                    }
                }
                data[j] = temp;
            }
            h /= 2;
        }
    }
    public static void Sort3<T>(T[] data) where T : System.IComparable<T>
    {
        int h = 1;
        while (h < data.Length) h = h * 3 + 1;

        while (h >= 1)
        {

            //堆 data[ start,start+h,start+2h ,start+3h...] 进行插入排序
            for (var i = h; i < data.Length; i++)
            {
                T temp = data[i];
                int j;
                for (j = i; j - h > 0; j -= h)
                {
                    if (data[j - h].CompareTo(temp) > 0)
                    {
                        data[j] = data[j - h];
                    }
                }
                data[j] = temp;
            }
            h /= 3;
        }
    }
}