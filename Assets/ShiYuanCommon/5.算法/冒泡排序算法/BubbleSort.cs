using System;
public class BubbleSort
{
    private BubbleSort()
    {
    }
    public static void Sort<T>(T[] data) where T : IComparable<T>
    {
        for (var i = 0; i < data.Length;)
        {
            int lastSwapIndex = 0;
            // arr[n-i ,n) 已经排序好了
            //通过冒泡在arr【n-i-1】 位置放在合适的元素
            for (var j = 0; j < data.Length - i - 1; j++)
            {
                if (data[j].CompareTo(data[j + 1]) > 0)
                {
                    Swap(data, j, j + 1);
                    lastSwapIndex = j + 1;
                }
            }
            i = data.Length - lastSwapIndex;
        }

    }
    public static void Sort2<T>(T[] data) where T : IComparable<T>
    {
        for (var i = 0; i + 1 < data.Length;)
        {
            int lastSwapIndex = data.Length - 1;
            for (var j = data.Length - 1; j > i; j--)
            {
                if (data[j].CompareTo(data[j - 1]) < 0)
                {
                    Swap(data, j, j - 1);
                    lastSwapIndex = j - 1;
                }
            }
            i = lastSwapIndex + 1;

        }

    }
    public static void Swap<T>(T[] data, int i, int j) where T : IComparable<T>
    {
        T temp = data[j];
        data[j] = data[i];
        data[i] = temp;
    }
}