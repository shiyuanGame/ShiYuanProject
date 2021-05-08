public interface Merger<T>
{
    T morge(T a, T b);

}

public class SegmentTree<T>
{
    private T[] data;
    public T[] tree;
    private Merger<T> merger;
    public SegmentTree(T[] arr, Merger<T> merger)
    {
        this.merger = merger;
        data = new T[arr.Length];
        for (var i = 0; i < arr.Length; i++)
        {
            data[i] = arr[i];
        }
        tree = new T[4 * arr.Length];
        buildSegmentTree(0, 0, data.Length - 1);
    }

    private void buildSegmentTree(int treeindex, int l, int r)
    {
        if (l == r)
        {
            tree[treeindex] = data[l];
            return;
        }
        int leftTreeIndex = leftChild(treeindex);
        int rightTreeIndex = rightChild(treeindex);
        int mid = l + (r - l) / 2;
        buildSegmentTree(leftTreeIndex, l, mid);
        buildSegmentTree(rightTreeIndex, mid + 1, r);

        tree[treeindex] = merger.morge(tree[leftTreeIndex], tree[rightTreeIndex]);

    }

    public int getSize()
    {
        return data.Length;
    }
    public T get(int index)
    {
        if (index < 0 || index >= data.Length)
            throw new System.Exception("Index illegal");
        return data[index];
    }
    public int leftChild(int index)
    {
        return 2 * index + 1;
    }
    public int rightChild(int index)
    {
        return 2 * index + 2;
    }
    public override string ToString()
    {

        System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
        stringBuilder.Append('[');
        for (var i = 0; i < tree.Length; i++)
        {
            if (tree[i] != null)
            {
                stringBuilder.Append(tree[i]);
            }
            else
            {
                stringBuilder.Append("null");
            }
            stringBuilder.Append(",");
        }
        stringBuilder.Append("]");
        return stringBuilder.ToString();
    }
    /// <summary>
    /// 返回区间 【queryL queryR】的值
    /// </summary>
    /// <param name="queryl"></param>
    /// <param name="queryR"></param>
    /// <returns></returns>
    public T query(int queryl, int queryR)
    {
        if (queryl < 0 || queryl >= data.Length || queryR < 0 || queryR >= data.Length || queryl > queryR)
            throw new System.Exception("index is illegil");
        return query(0, 0, data.Length - 1, queryl, queryR);
    }
    private T query(int treeindex, int l, int r, int queryl, int queryR)
    {
        if (l == queryl && r == queryR)
        {
            return tree[treeindex];
        }
        int mid = l + (r - l) / 2;
        int leftIndex = leftChild(treeindex);
        int rightIndex = rightChild(treeindex);
        if (queryl >= mid + 1)
        {
            return query(rightIndex, mid + 1, r, queryl, queryR);
        }
        else if (queryR <= mid)
        {
            return query(leftIndex, l, mid, queryl, queryR);
        }
        T leftResult = query(leftIndex, l, mid, queryl, mid);
        T rightResult = query(rightIndex, mid + 1, r, mid + 1, queryR);
        return merger.morge(leftResult, rightResult);
    }
    public void set(int index, T data)
    {
        if (index < 0 || index >= this.data.Length)
        {
            throw new System.Exception(" index is illegal");
        }
        set(0, 0, this.data.Length - 1, index, data);
    }
    private void set(int treeIndex, int l, int r, int index, T data)
    {
        if (l == r)
        {
            tree[treeIndex] = data;
            return;
        }
        int mid = l + (r - l) / 2;
        int leftTreeIndex = leftChild(treeIndex);
        int rightTreeIndex = rightChild(treeIndex);
        if (index >= mid + 1)
        {
            set(rightTreeIndex, mid + 1, r, index, data);
        }
        else
        {
            set(leftTreeIndex, l, mid, index, data);
        }
        tree[treeIndex] = merger.morge(tree[leftTreeIndex], tree[rightTreeIndex]);
    }
}