public class QuickSort
{
    private QuickSort() { }
    public static void Sort<T>(T[] arr) where T : System.IComparable<T>
    {
        Sort(arr, 0, arr.Length - 1, new System.Random());

    }
    public static void Sort2<T>(T[] arr) where T : System.IComparable<T>
    {
        Sort2(arr, 0, arr.Length - 1, new System.Random());

    }
    private static void Sort<T>(T[] arr, int l, int r, System.Random random) where T : System.IComparable<T>
    {
        // 改为插入排序
        if (l >= r) return;
        int p = partition(arr, l, r, random);
        Sort(arr, l, p - 1, random);
        Sort(arr, p + 1, r, random);
    }
    private static void Sort2<T>(T[] arr, int l, int r, System.Random random) where T : System.IComparable<T>
    {
        // 改为插入排序
        // if (r - l <= 8)
        // {
        //     // 调用 插入排序算法类中的方法 如果不存在就调用下面的
        //     InsertionSort.Sort(arr, l, r);
        //     //本类插入排序
        //     // insertionSort(arr, l, r);
        //     return;
        // }

        if (l >= r)
        {
            // 调用 插入排序算法类中的方法 如果不存在就调用下面的
            // InsertionSort.Sort(arr, l, r);
            //本类插入排序
            // insertionSort(arr, l, r);
            return;
        }
        int p = partition(arr, l, r, random);
        Sort2(arr, l, p - 1, random);
        Sort2(arr, p + 1, r, random);
    }
    private static int partition<T>(T[] arr, int l, int r, System.Random random) where T : System.IComparable<T>
    {
        //随机 l 到r之间的随机数
        //   System.Random random = new System.Random();
        int p = l + random.Next(r - l + 1);

        swap(arr, l, p);

        //  left  arr[l+1....j] <v  right arr[j+1....i]>= v
        int j = l;
        for (var i = l + 1; i <= r; i++)
        {
            if (arr[i].CompareTo(arr[l]) < 0)
            {
                j++;
                swap(arr, i, j);

            }
        }
        swap(arr, l, j);
        return j;
    }

    private static void swap<T>(T[] arr, int i, int j) where T : System.IComparable<T>
    {
        var temp = arr[i];
        arr[i] = arr[j];
        arr[j] = temp;

    }
    public static void insertionSort<T>(T[] data, int l, int r) where T : System.IComparable<T>
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
    public static void Sort2Ways<T>(T[] arr) where T : System.IComparable<T>
    {
        Sort2Ways(arr, 0, arr.Length - 1, new System.Random());

    }
    private static void Sort2Ways<T>(T[] arr, int l, int r, System.Random random) where T : System.IComparable<T>
    {
        // 改为插入排序
        if (r - l <= 2)
        {
            // 调用 插入排序算法类中的方法 如果不存在就调用下面的
            // InsertionSort.Sort(arr, l, r);
            //本类插入排序
            insertionSort(arr, l, r);
            return;
        }

        int p = partition2Ways(arr, l, r, random);
        Sort2Ways(arr, l, p - 1, random);
        Sort2Ways(arr, p + 1, r, random);
    }
    private static int partition2Ways<T>(T[] arr, int l, int r, System.Random random) where T : System.IComparable<T>
    {
        //随机 l 到r之间的随机数
        //   System.Random random = new System.Random();
        int p = l + random.Next(r - l + 1);

        swap(arr, l, p);
        int i = l + 1, j = r;
        //  left  arr[l+1....j] <=v  right arr[j+1....i]>= v
        while (true)
        {
            //循环终止条件是 当前的i 》=0  循环条件是《0
            while (i <= j && arr[i].CompareTo(arr[l]) < 0)
            {
                i++;

            }
            while (j >= i && arr[j].CompareTo(arr[l]) > 0)
            {
                j--;
            }
            if (i >= j)
            {
                break;
            }
            swap(arr, i, j);
            i++;
            j--;
        }
        swap(arr, l, j);
        return j;
    }
    public static void Sort3Ways<T>(T[] arr) where T : System.IComparable<T>
    {
        Sort3Ways(arr, 0, arr.Length - 1, new System.Random());

    }
    private static void Sort3Ways<T>(T[] arr, int l, int r, System.Random random) where T : System.IComparable<T>
    {
        if (l >= r) return;
        var p = partition3Ways(arr, l, r, random);
        Sort3Ways(arr, l, p.Item1, random);
        Sort3Ways(arr, p.Item2, r, random);
    }
    private static (int, int) partition3Ways<T>(T[] arr, int l, int r, System.Random random) where T : System.IComparable<T>
    {
        //随机 l 到r之间的随机数
        int p = l + random.Next(r - l + 1);

        swap(arr, l, p);
        int lt = l, i = l + 1, gt = r + 1;
        // arr[l+1, lt] <v   arr [ lt+1,i-1]==v   arr[gr,r]>v
        while (i < gt)
        {
            if (arr[i].CompareTo(arr[l]) < 0)
            {
                lt++;
                swap(arr, i, lt);
                i++;
            }
            else if (arr[i].CompareTo(arr[l]) == 0)
            {
                i++;
            }
            else if (arr[i].CompareTo(arr[l]) > 0)
            {
                gt--;
                swap(arr, i, gt);
            }
        }
        swap(arr, l, lt);
        // arr[l, lt-1] <v   arr [ lt,i-1]==v   arr[gr,r]>v

        return (lt - 1, gt);
    }
};