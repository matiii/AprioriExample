using System;
using System.Collections.Generic;
using System.Linq;

namespace Apriori.App.Structure
{
    class Node : Dictionary<int, Node>
    {
        private readonly List<Leaf> _leafs = new List<Leaf>();

        private readonly int _maxSize;

        public Node Parent { get; }
        public int Level { get; }
        public int Key { get; }

        public Leaf[] Leafs => _leafs.ToArray();
        public bool IsRoot => Parent == null;

        public Node(int maxSize) //set root
        {
            _maxSize = maxSize;
            Level = 0;
            Key = -1;
        }

        public Node(Node parent, int[] elements, int maxSize): this(maxSize)
        {
            Parent = parent;
            elements = elements.Clean();
            Level = parent.Level + 1;
            Key = GetHashCode(elements[parent.Level]);
        }

        public void Add(int[] elements)
        {
            var que = new Queue<Node>();
            que.Enqueue(this);

            while (que.Count > 0)
            {
                var current = que.Dequeue();

                var adddedKeys = new List<int>();

                for (int i = 0; i < elements.Length - current.Level; i++)
                {
                    int[] items = elements.Skip(i).ToArray();


                    int element = items[current.Level];
                    int key = GetHashCode(element);

                    if (adddedKeys.Contains(key)) continue;
                    
                    adddedKeys.Add(key);                        

                    Node node;

                    if (!current.ContainsKey(key))
                    {
                        node = new Node(current, items, _maxSize);
                        current.Add(key, node);
                    }
                    else
                        node = current[key];

                    if (node.Level < _maxSize && node.Level < items.Length)
                        que.Enqueue(node);

                    node.GenerateLeafs(elements);
                }
            }
        }


        public void GenerateLeafs(int[] elements)
        {
            int[] items = GetNodeElements(elements);

            var que = new Queue<List<int>>();

            foreach (var element in elements)
                que.Enqueue(new List<int> { element });

            while (que.Count > 0)
            {
                var job = que.Dequeue();

                if (job.Count == Level)
                {
                    if (!items.Contains(job.Last())) continue;
                    if (Parent.Leafs.Length > 0 && !Parent.Leafs.Any(x => x.Exist(job.Take(Level - 1).ToArray()))) continue;

                    var leaf = _leafs.FirstOrDefault(x => x.Exist(job.ToArray()));

                    if (leaf == null)
                        _leafs.Add(new Leaf { Attempts = 1, Elements = job.ToArray() });
                    else
                        leaf.Attempts++;
                }
                else
                {
                    int index = Array.IndexOf(elements, job.Last()) + 1;

                    for (int i = index; i < elements.Length; i++)
                        que.Enqueue(new List<int>(job) { elements[i] });
                }
            }
        }

        private int[] GetNodeElements(int[] elements) => elements.Where(element => GetHashCode(element) == Key).ToArray();
        

        private int GetHashCode(int element) => element % _maxSize;

        public override string ToString()
        {
            return $"[Level: {Level}] [Key: {Key}]";
        }
    }
}
