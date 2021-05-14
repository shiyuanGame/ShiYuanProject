interface UF
{
    int getSize();
    bool isConnected(int p, int q);
    void unionElements(int p, int q);
}
public class QuickUnionFind : UF
{
    private int[] parent;
    private int[] rank;

    public QuickUnionFind(int size)
    {
        parent = new int[size];
        rank = new int[size];
        for (var i = 0; i < size; i++)
        {
            parent[i] = i;
            rank[i] = 1;
        }
    }

    public int getSize()
    {
        return parent.Length;
    }
    /// <summary>
    /// 查找过程 查找元素p所对应的集合编号
    /// O(h）复杂度 h为树的高度
    /// </summary>
    /// <param name="p"></param>
    /// <returns></returns>
    private int find2(int p)
    {
        if (p < 0 || p >= parent.Length)
            throw new System.Exception("p is out of bound");
        while (p != parent[p])
        {
            //路径压缩
            parent[p] = parent[parent[p]];
            p = parent[p];
        }
        return p;
    }
    /// <summary>
    /// 查找过程 查找元素p所对应的集合编号
    /// O(h）复杂度 h为树的高度
    /// </summary>
    /// <param name="p"></param>
    /// <returns></returns>
    private int find(int p)
    {
        if (p < 0 || p >= parent.Length)
            throw new System.Exception("p is out of bound");
        if (p != parent[p])
        {
            // 递归 内部运行 路径压缩  
            parent[p] = find(parent[p]);
        }
        return parent[p];
    }
    /// <summary>
    /// 查看元素p和q是否所属一个集合
    /// O（h）复杂度  h为树的高度
    /// </summary>
    /// <param name="p"></param>
    /// <param name="q"></param>
    /// <returns></returns>
    public bool isConnected(int p, int q)
    {
        return find(p) == find(q);
    }

    public void unionElements(int p, int q)
    {
        int pRoot = find(p);
        int qRoot = find(q);
        if (pRoot == qRoot) return;
        //根据两个元素所在树的rank不同判断合并方向
        //讲rank低的集合合并到runk高的集合上
        if (rank[pRoot] < rank[qRoot])
        {
            parent[pRoot] = qRoot;
        }
        else if (rank[qRoot] < rank[pRoot])
        {
            parent[qRoot] = pRoot;
        }
        else
        {
            parent[qRoot] = pRoot;
            rank[pRoot] += 1;
        }
    }



}