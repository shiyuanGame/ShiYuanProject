using System;
using System.Text;

public class MergeSort
{
    private MergeSort()
    {

    }
    public static void Sort<T>(T[] arr) where T : IComparable<T>
    {
        Sort(arr, 0, arr.Length - 1, 0);
    }
    private static string generateDepthString(int count)
    {

        StringBuilder stringBuilder = new StringBuilder();
        for (var i = 0; i < count; i++)
        {
            stringBuilder.Append("-");
        }
        return stringBuilder.ToString();
    }
    private static void Sort<T>(T[] arr, int l, int r, int depth) where T : IComparable<T>
    {
        // 生成深度字符串
        string depthString = generateDepthString(depth);

        // 打印当前 sort 处理的数组区间信息
        // System.Console.Write(depthString);
        // System.Console.WriteLine(string.Format("mergesort arr[{0}, {1}]", l, r));



        if (l >= r) return;
        int mid = l + (r - l) / 2;
        Sort(arr, l, mid, depth + 1);
        Sort(arr, mid + 1, r, depth + 1);


        // 打印这次 merge 要处理的区间范围
        // System.Console.Write(depthString);
        // System.Console.WriteLine(string.Format("merge arr[{0}, {1}] and arr[{2}, {3}]", l, mid, mid + 1, r));
        morge(arr, l, mid, r);
        // // 打印 merge 后的数组
        // System.Console.Write(depthString);
        // System.Console.WriteLine(string.Format("after mergesort arr[{0}, {1}] :", l, r));
        // for (E e: arr)
        //     System.out.print(e + " ");
        // System.out.println();
        // for (var i = 0; i < arr.Length; i++)
        // {
        //     System.Console.Write(" " + arr[i]);
        // }
        // System.Console.WriteLine();
    }
    //合并两个有序的区间arr[l,mid] arr[mid+1 ,r]
    private static void morge<T>(T[] arr, int l, int mid, int r) where T : IComparable<T>
    {
        T[] tempArr = new T[r - l + 1];
        // T[] temp = System.Array.Copy()
        // System.Array.Copy( arr,temp,)

        int copynum = l;
        for (var m = 0; m < tempArr.Length; m++)
        {
            tempArr[m] = arr[copynum++];
        }

        int i = l, j = mid + 1;
        //对arr[k] 进行赋值
        for (var k = l; k <= r; k++)
        {
            if (i > mid)
            {
                arr[k] = tempArr[j - l]; j++;
            }
            else if (j > r)
            {
                arr[k] = tempArr[i - l]; i++;
            }
            else if (tempArr[i - l].CompareTo(tempArr[j - l]) <= 0)
            {
                arr[k] = tempArr[i - l]; i++;
            }
            else
            {
                arr[k] = tempArr[j - l]; j++;
            }

        }
    }
    /// <summary>
    /// 优化版 归并排序 算法
    /// </summary>
    /// <param name="arr"></param>
    /// <typeparam name="T"></typeparam>
    public static void Sort2<T>(T[] arr) where T : IComparable<T>
    {
        T[] tempArr = new T[arr.Length];
        System.Array.Copy(arr, tempArr, arr.Length);
        Sort2(arr, tempArr, 0, arr.Length - 1, 0);
    }

    private static void Sort2<T>(T[] arr, T[] tempArr, int l, int r, int depth) where T : IComparable<T>
    {
        // 生成深度字符串
        // string depthString = generateDepthString(depth);

        // 打印当前 sort 处理的数组区间信息
        // System.Console.Write(depthString);
        // System.Console.WriteLine(string.Format("mergesort arr[{0}, {1}]", l, r));

        // if (l >= r) return;
        // 改为插入排序
        if (r - l <= 15)
        {
            // 调用 插入排序算法类中的方法 如果不存在就调用下面的
            // InsertionSort.Sort(arr, l, r);
            //本类插入排序
            insertionSort(arr, l, r);
            return;
        }
        int mid = l + (r - l) / 2;
        Sort2(arr, tempArr, l, mid, depth + 1);
        Sort2(arr, tempArr, mid + 1, r, depth + 1);

        // 打印这次 merge 要处理的区间范围
        // System.Console.Write(depthString);
        // System.Console.WriteLine(string.Format("merge arr[{0}, {1}] and arr[{2}, {3}]", l, mid, mid + 1, r));
        if (arr[mid].CompareTo(arr[mid + 1]) < 0) return;
        morge2(arr, tempArr, l, mid, r);
        // 打印 merge 后的数组
        // System.Console.Write(depthString);
        // System.Console.WriteLine(string.Format("after mergesort arr[{0}, {1}] :", l, r));

        // for (var i = 0; i < arr.Length; i++)
        // {
        //     System.Console.Write(" " + arr[i]);
        // }
        // System.Console.WriteLine();
    }
    //合并两个有序的区间arr[l,mid] arr[mid+1 ,r]

    //插入排序算法
    private static void morge2<T>(T[] arr, T[] tempArr, int l, int mid, int r) where T : IComparable<T>
    {
        System.Array.Copy(arr, l, tempArr, l, r - l + 1);

        int copynum = l;

        int i = l, j = mid + 1;
        //对arr[k] 进行赋值
        for (var k = l; k <= r; k++)
        {
            if (i > mid)
            {
                arr[k] = tempArr[j]; j++;
            }
            else if (j > r)
            {
                arr[k] = tempArr[i]; i++;
            }
            else if (tempArr[i].CompareTo(tempArr[j]) <= 0)
            {
                arr[k] = tempArr[i]; i++;
            }
            else
            {
                arr[k] = tempArr[j]; j++;
            }

        }
    }
    public static void insertionSort<T>(T[] data, int l, int r) where T : IComparable<T>
    {
        for (var i = l; i <= r; i++)
        {
            T temp = data[i];
            int j;
            for (j = i; j > l; j--)
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
    //自低向上的归并排序算法
    public static void SortBU<T>(T[] data) where T : IComparable<T>
    {
        int n = data.Length;
        T[] temparray = new T[n];

        System.Array.Copy(data, temparray, n);
        for (var i = 0; i < n; i += 16)
        {
            // InsertionSort.Sort(data, i, Math.Min(n - 1, i + 15));
            //或者
            insertionSort(data, i, Math.Min(n - 1, i + 15));
        }
        // 遍历合并的区间长度
        // 注意，sz 从 16 开始
        for (var sz = 16; sz < n; sz += sz)
        {
            for (var i = 0; i + sz < n; i += sz + sz)
            {
                if (data[i + sz - 1].CompareTo(data[i + sz]) > 0)
                    morge2(data, temparray, i, i + sz - 1, Math.Min(n - 1, i + sz + sz - 1));
            }
        }

    }
}