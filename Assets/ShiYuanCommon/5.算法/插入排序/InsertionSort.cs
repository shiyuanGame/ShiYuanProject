using System;
public class InsertionSort
{
    private InsertionSort()
    { }
    public static void Sort<T>(T[] data) where T : IComparable<T>
    {
        for (var i = 0; i < data.Length; i++)
        {
            for (var j = i; j > 0; j--)
            {
                if (data[j].CompareTo(data[j - 1]) < 0)
                {
                    Swap(data, j, j - 1);
                }
                else
                {
                    break;
                }
            }
        }
    }
    //优化版本
    public static void Sort2<T>(T[] data) where T : IComparable<T>
    {
        for (var i = 0; i < data.Length; i++)
        {
            T temp = data[i];
            int j;
            for (j = i; j > 0; j--)
            {
                if (temp.CompareTo(data[j - 1]) < 0)
                {
                    data[j] = data[j - 1];
                }
                else
                {
                    break;
                }
            }
            data[j] = temp;
        }
    }
    static void Swap<T>(T[] data, int i, int j)
    {
        T temp = data[i];
        data[i] = data[j];
        data[j] = temp;
    }
}