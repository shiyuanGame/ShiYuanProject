
using System.Collections;
using System.Collections.Generic;
using System.Text;
/// <summary>
/// 二分搜索树
/// </summary>
public class BST<T> where T : System.IComparable<T>
{
    public class Node
    {
        public T data;
        public Node left;
        public Node right;
        public Node(T data)
        {
            this.data = data;
            left = null;
            right = null;

        }
    }

    private Node root;
    private int _size;
    public BST()
    {
        _size = 0;
        root = null;
    }
    public int size()
    {
        return _size;
    }
    public bool isEnmpty()
    {
        return _size == 0;
    }
    public Node add(T data)
    {
        return root = add(root, data);
    }
    private Node add(Node node, T data)
    {
        if (node == null)
        {
            _size++;
            return new Node(data);
        }
        if (data.CompareTo(node.data) < 0)
        {
            node.left = add(node.left, data);
        }
        else if (data.CompareTo(node.data) > 0)
        {
            node.right = add(node.right, data);
        }
        return node;
    }

    private void add2(T data)
    {
        if (root == null)
        {
            _size++;
            root = new Node(data);
            return;
        }

        var p = root;
        while (p != null)
        {
            if (data.CompareTo(p.data) > 0)
            {
                if (p.right == null)
                {
                    p.right = new Node(data);
                    _size++;
                    return;
                }
                p = p.right;

            }
            else if (data.CompareTo(p.data) < 0)
            {
                if (p.left == null)
                {
                    p.left = new Node(data);
                    _size++;
                    return;
                }
                p = p.left;
            }
            else
            {
                return;
            }
        }


    }
    public bool Contains(T data)
    {
        return Contains(root, data);
    }
    private bool Contains(Node node, T data)
    {
        if (node == null)
        {
            return false;

        }
        if (data.CompareTo(node.data) == 0)
        {
            return true;
        }
        else if (data.CompareTo(node.data) < 0)
        {
            return Contains(node.left, data);
        }
        else
        {
            return Contains(node.right, data);
        }
    }
    /// <summary>
    /// 前序遍历
    /// </summary>
    public void preOcder()
    {
        preOcder(root);

    }
    private void preOcder(Node node)
    {
        if (node == null) return;
        System.Console.WriteLine(node.data);
        preOcder(node.left);
        preOcder(node.right);

    }
    /// <summary>
    /// 中序遍历
    /// </summary>
    private void inOrder()
    {
        inOrder(root);
    }
    private void inOrder(Node node)
    {
        if (node == null)
        {
            return;
        }
        inOrder(node.left);
        System.Console.WriteLine(node.data);
        inOrder(node.right);
    }
    /// <summary>
    /// 后序遍历
    /// </summary>
    public void postOder()
    {
        postOder(root);
    }
    private void postOder(Node node)
    {
        if (node == null)
            return;
        postOder(node.left);
        postOder(node.right);
        System.Console.WriteLine(node.data);
    }
    /// <summary>
    /// 非递归前序遍历  使用栈
    /// </summary>
    public void preOderNR()
    {
        Stack<Node> stack = new Stack<Node>();
        stack.Push(root);
        while (stack.Count != 0)
        {
            var curr = stack.Pop();
            System.Console.WriteLine(curr.data);
            if (curr.right != null)
                stack.Push(curr.right);
            if (curr.left != null)
                stack.Push(curr.left);
        }
    }
    /// <summary>
    /// 层序遍历
    /// /// </summary>
    public void levelOrder()
    {
        Queue<Node> queue = new Queue<Node>();
        queue.Enqueue(root);
        while (queue.Count != 0)
        {
            var curr = queue.Dequeue();
            System.Console.WriteLine(curr.data);
            if (curr.left != null)
                queue.Enqueue(curr.left);
            if (curr.right != null)
                queue.Enqueue(curr.right);
        }
    }
    public Node minNum()
    {
        if (_size == 0)
            throw new System.Exception(" BSP is empty！");
        return minNum(root);
    }
    private Node minNum(Node node)
    {
        if (node.left == null)
        {
            return node;
        }
        return minNum(node.left);
    }
    /// <summary>
    /// 最大值
    /// </summary>
    /// <returns></returns>
    public Node maxNum()
    {
        if (_size == 0)
            throw new System.Exception(" BSP is empty！");
        return maxNum(root);
    }
    private Node maxNum(Node node)
    {
        if (node.right == null)
        {
            return node;
        }
        return maxNum(node.right);
    }
    private void remove(T data)
    {
        root = remove(root, data);
    }
    /// <summary>
    /// 删除以node为根的二分搜索树中 值为data的节点 递归因算
    /// 返回删除节点后新的二分搜索树
    /// </summary>
    /// <param name="node"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    private Node remove(Node node, T data)
    {
        if (node == null) return null;
        if (data.CompareTo(node.data) < 0)
        {
            node.left = remove(node.left, data);
            return node;
        }
        else if (data.CompareTo(node.data) > 0)
        {
            node.right = remove(node.right, data);
            return node;
        }
        else
        { //data== node.data
            if (node.left == null)
            {
                Node rightnode = node.right;
                node.right = null;
                _size--;
                return rightnode;
            }
            else if (node.right == null)
            {
                Node leftNode = node.left;
                leftNode.left = null;
                _size--;
                return leftNode;
            }
            /// <summary>
            /// 待删除节点的左右子树均不为空
            /// 找到比这个删除节点打的最小节点 即删除右子树的最小节点
            /// 用这个节点顶替删除节点的位置
            /// </summary>
            /// <returns></returns>
            Node successor = minNum(node.right);
            successor.right = removeMin(node.right);
            successor.left = node.right;
            node.left = node.right = null;
            return successor;
        }

    }
    public T removeMin()
    {
        Node min = minNum();
        root = removeMin(root);
        return min.data;
    }
    /// <summary>
    /// 删除 掉以node为根的二分搜索树中最小的节点
    /// 但会删除节点以后 新的二分搜索树的跟
    /// /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    private Node removeMin(Node node)
    {
        if (node.left == null)
        {
            Node rightNode = node.right;
            node.right = null;
            _size--;
            return rightNode;
        }
        node.left = removeMin(node.left);
        return node;
    }
    public T removeMax()
    {
        var max = maxNum();
        System.Console.WriteLine("max :" + max);
        root = removeMax(root);
        return max.data;
    }
    private Node removeMax(Node node)
    {
        if (node.right == null)
        {
            Node leftNode = node.left;
            node.left = null;
            _size--;
            return leftNode;
        }
        node.right = removeMax(node.right);
        return node;
    }
    public override string ToString()
    {

        StringBuilder stringBuilder = new StringBuilder();
        // stringBuilder.Append()
        GenerateBSTString(root, 0, stringBuilder);
        return stringBuilder.ToString();
    }
    private void GenerateBSTString(Node node, int depth, StringBuilder sb)
    {
        if (node == null)
        {
            sb.Append(generateDepthString(depth) + "null\n");
            return;
        }
        sb.Append(generateDepthString(depth) + node.data + "\n");
        GenerateBSTString(node.left, depth + 1, sb);
        GenerateBSTString(node.right, depth + 1, sb);
        // depth++;


    }
    private string generateDepthString(int depth)
    {
        StringBuilder stringBuilder = new StringBuilder();
        for (var i = 0; i < depth; i++)
        {
            stringBuilder.Append(" -");
        }
        return stringBuilder.ToString();
    }
}