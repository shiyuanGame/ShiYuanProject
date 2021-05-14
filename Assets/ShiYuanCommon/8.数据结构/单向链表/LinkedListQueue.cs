namespace shiyuan {
    interface Queue<T>
    {
        int getSize();
        bool isEmpty();
        void enqueue(T data);
        T dequeue();
        /// <summary>
        /// 查看栈顶元素
        /// </summary>
        /// <returns></returns>
        T getFront();
    }
    public class LinkedListQueue<T> : Queue<T> {
        public class Node {
            public T data;
            public Node next;
            public Node (T data, Node next) {
                this.data = data;
                this.next = next;
            }
            public Node (T data) : this (data, null) {

            }
            public Node () : this (default (T), null) { }
            public override string ToString () {
                return data.ToString ();
            }
        }
        public Node head,
        tail;
        private int size;
        public LinkedListQueue () {
            head = null;
            tail = null;
            size = 0;
        }
        public T dequeue () {
            if (isEmpty ()) {
                throw new System.Exception ("size is 0  no dequue");
            }
            Node node = head;
            head = node.next;
            node.next = null;
            if (head == null) {
                tail = null;
            }
            size--;
            return node.data;
        }

        public void enqueue (T data) {
            //如果头是空的情况下 tail 才是空的
            if (tail == null) {
                tail = new Node (data);
                head = tail;
            } else {
                tail.next = new Node (data);
                this.tail = tail.next;
            }
            size++;
        }

        public T getFront () {
            if (isEmpty ()) {
                throw new System.Exception ("size is 0  no dequue");
            }
            return head.data;
        }

        public int getSize () {
            return size;
        }

        public bool isEmpty () {
            return size == 0;
        }
        public override string ToString () {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder ();
            stringBuilder.Append ("queue : front");
            Node cur = head;
            while (cur != null) {
                stringBuilder.Append (cur + "->");
                cur = cur.next;

            }
            stringBuilder.Append ("Null tail");
            return stringBuilder.ToString ();
        }
    }

    interface Stack<T> {
        int getSize ();
        bool isEmpty ();
        void push (T data);
        T pop ();
        /// <summary>
        /// 查看栈顶元素
        /// </summary>
        /// <returns></returns>
        T peek ();
    }

    public class LinkedListStack<T> : Stack<T> {
        private LinkedList<T> list;
        public LinkedListStack () {
            list = new LinkedList<T> ();
        }
        public int getSize () {
            return list.getSize ();
        }

        public bool isEmpty () {
            return list.getSize () == 0;
        }

        public T peek () {
            return list.getFirst ();
        }

        public T pop () {
            return list.removeFirst ();
        }

        public void push (T data) {
            list.addFirst (data);
        }
        public override string ToString () {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder ();
            stringBuilder.Append (" stack :top");
            stringBuilder.Append (list);
            return stringBuilder.ToString ();
        }
    }

    public class LinkedList<T> {
        private class Node {
            public T data;
            public Node next;

            public Node (T data, Node next) {
                this.data = data;
                this.next = next;
            }
            public Node (T data) : this (data, null) {

            }
            public Node () : this (default (T), null) { }
            public override string ToString () {
                return data.ToString ();
            }
        }
        // private Node head = null;
        /// <summary>
        /// 虚拟头结点
        /// </summary>
        private Node dummyHead = null;
        private int size;
        public LinkedList () {
            // head = null;
            dummyHead = new Node ();
            size = 0;
        }
        public int getSize () {
            return size;
        }
        public bool usEmpty () {
            return size == 0;
        }
        public void addFirst (T data) {
            Add (data, 0);
        }
        public void Add (T data, int index) {
            if (index < 0 || index > size)
                throw new System.Exception ("add failed,allegal index");
            Node prev = dummyHead;
            for (var i = 0; i < index; i++) {
                prev = prev.next;
            }
            // System.Console.WriteLine(data);
            prev.next = new Node (data, prev.next);
            size++;
        }
        public void AddLast (T data) {
            Add (data, size);
        }
        public T get (int index) {
            if (index < 0 || index >= size)
                throw new System.Exception ("get failed illegal index.");

            Node curr = dummyHead.next;
            for (var i = 0; i < index; i++) {
                curr = curr.next;
            }
            return curr.data;
        }
        public T getFirst () {
            return get (0);
        }
        public T getLast () {
            return get (size - 1);
        }
        public void Set (int index, T data) {
            if (index < 0 || index >= size)
                throw new System.Exception ("set failed illegal index.");
            Node curr = dummyHead.next;
            for (var i = 0; i < index; i++) {
                curr = curr.next;
            }
            curr.data = data;
        }
        public bool contains (T data) {
            Node curr = dummyHead.next;
            while (curr != null) {
                if (curr.data.Equals (data)) {
                    return true;
                }

            }
            return false;
        }
        public override string ToString () {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder ();
            Node curr = dummyHead.next;
            while (curr != null) {
                stringBuilder.Append (curr + "=>");
                curr = curr.next;
            }
            stringBuilder.Append ("NULL");
            return stringBuilder.ToString ();
        }
        public T Remove (int index) {
            Node prev = dummyHead;
            for (var i = 0; i < index; i++) {
                prev = prev.next;
            }
            Node node = prev.next;
            prev.next = node.next;
            node.next = null;
            size--;
            return node.data;
        }
        public T removeFirst () {
            return Remove (0);
        }
        public T removeLast () {
            return Remove (size - 1);
        }
    }
}