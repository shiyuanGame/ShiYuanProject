using System;

public class SelectionSort
{
    private SelectionSort()
    {

    }
    public static void Sort<T>(T[] arr) where T : IComparable<T>
    {
        for (var i = 0; i < arr.Length; i++)
        {
            int minIndex = i;
            for (var j = i; j < arr.Length; j++)
            {
                if (arr[j].CompareTo(arr[minIndex]) < 0)
                {
                    minIndex = j;
                }
            }
            Swap(arr, minIndex, i);
        }
    }
    static void Swap<T>(T[] arr, int i, int j)
    {
        T tempi = arr[i];
        arr[i] = arr[j];
        arr[j] = tempi;
    }
}