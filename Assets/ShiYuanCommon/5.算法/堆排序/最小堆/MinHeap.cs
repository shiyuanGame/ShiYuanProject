using System;

using System.Collections.Generic;
/// <summary>
/// 最大堆   优先级
/// </summary>
/// <typeparam name="T"></typeparam>
public class MinHeap<T> where T : IComparable<T>
{
    public List<T> data;
    public MinHeap(int capacity)
    {
        data = new List<T>(capacity);
    }
    public MinHeap()
    {
        data = new List<T>();

    }
    /// <summary>
    /// Heapify 操作
    /// </summary>
    /// <param name="arr"></param>
    public MinHeap(T[] arr)
    {
        this.data = new List<T>(arr);
        for (var i = parent(size() - 1); i >= 0; i--)
        {
            SiftDown(i);
        }

    }
    public int size()
    {
        return data.Count;
    }
    public bool isEmpty()
    {
        return data.Count == 0;
    }
    //但会完全二叉树的数组中标识一个索引所标识的元素的父亲节点的元素的索引
    private int parent(int index)
    {
        if (index <= 0)
        {
            throw new Exception(" index-0 don't hace parnt");
        }
        return (index - 1) / 2;
    }
    private int leftChild(int parent)
    {
        return parent * 2 + 1;
    }
    private int rightChild(int parent)
    {
        return parent * 2 + 2;
    }
    public void Add(T data)
    {
        this.data.Add(data);

        SiftUp(this.data.Count - 1);

    }
    /// <summary>
    ///  上浮
    /// </summary>
    public void SiftUp(int k)
    {
        // K 不能越界 并且 当前的节点大于父亲节点
        while (k > 0 && data[parent(k)].CompareTo(data[k]) > 0)
        {
            /// <summary>
            /// 交换赋值
            /// </summary>
            /// <returns></returns>
            Swap(k, parent(k));
            k = parent(k);
        }
    }
    public void Swap(int i, int j)
    {
        if (i < 0 || i >= size() || j < 0 || j >= size())
        {
            throw new Exception("  Index is illegal.");
        }
        var temp = data[i];
        data[i] = data[j];
        data[j] = temp;
    }
    public T findMin()
    {
        if (isEmpty())
        {
            throw new Exception(" can dot findMax whenheap is empty");
        }
        return data[0];
    }
    public T ExtractMin()
    {
        T temp = findMin();
        Swap(0, data.Count - 1);
        data.RemoveAt(size() - 1);
        SiftDown(0);
        return temp;
    }
    /// <summary>
    /// 下沉操作
    /// </summary>
    /// <param name="k"></param>
    private void SiftDown(int k)
    {
        //左边孩子的索引 不能超出索引 如果存在左边索引 的话
        while (leftChild(k) < size())
        {
            int j = leftChild(k);
            //存在右边孩子 并且 右边孩子比左边小
            if (j + 1 < size() && data[j].CompareTo(data[j + 1]) > 0)
            {
                j = rightChild(k);
            }
            //data[j]是left right child 中的最小值
            if (data[k].CompareTo(data[j]) <= 0) break;

            Swap(k, j);
            k = j;

        }
    }
    /// <summary>
    /// 去除最小元素后 放入一个新的元素
    /// </summary>
    public T replace(T _data)
    {
        T temp = findMin();
        data[0] = _data;
        SiftDown(0);
        return temp;
    }

}