using System;
using System.Collections;
/// <summary>
/// 给予堆排序 优先队列
/// </summary>
public class PriorityQueue<T> where T : IComparable<T>
{
    private MaxHeap<T> maxHeap;
    private PriorityQueue()
    {
        maxHeap = new MaxHeap<T>();
    }
    public int getSize()
    {
        return maxHeap.size();
    }
    public T getFront()
    {
        return maxHeap.findMax();
    }
    public void Enqueue(T data)
    {
        maxHeap.Add(data);
    }

    public T Dequeue()
    {
        return maxHeap.ExtractMax();
    }
}