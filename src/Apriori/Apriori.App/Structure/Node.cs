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
            var addedKeys = new List<int>();

            for (int i = 0; i < elements.Length; i++)
            {
                int key = GetHashCode(elements[i]);

                if (addedKeys.Contains(key)) continue;
                
                addedKeys.Add(key);
                
                Process(elements.Skip(i).ToArray());                    
            }
        }

        private void Process(int[] elements)
        {
            var que = new Queue<Node>();
            que.Enqueue(this);

            while (que.Count > 0)
            {
                var current = que.Dequeue();

                int element = elements[current.Level];
                int key = GetHashCode(element);

                Node node;

                if (!current.ContainsKey(key))
                {
                    node = new Node(current, elements, _maxSize);
                    current.Add(key, node);
                }
                else
                    node = current[key];

                if (node.Level < _maxSize && node.Level < elements.Length)
                    que.Enqueue(node);

                node.GenerateLeafs(elements.Skip(current.Level).ToArray());
            }
        }

        //TODO: optimize
        public void GenerateLeafs(int[] elements)
        {
            int[] items = GetNodeElements(elements).ToArray();

            var que = new Queue<List<int>>();

            foreach (var item in items)
                que.Enqueue(new List<int> { item });

            while (que.Count > 0)
            {
                var job = que.Dequeue();

                if (job.Count == Level)
                {
                    var leaf = _leafs.FirstOrDefault(x => x.Exist(job.ToArray()));

                    if (leaf == null)
                        _leafs.Add(new Leaf { Attempts = 1, Elements = job.ToArray() });
                    else
                        leaf.Attempts++;
                }
                else
                {
                    int index = Array.IndexOf(items, job.Last()) + 1;

                    for (int i = index; i < items.Length; i++)
                        que.Enqueue(new List<int>(job) { items[i] });
                }
            }
        }

        private int[] GetNodeElements(int[] elements)
        {
            List<int> result = elements.Where(element => GetHashCode(element) == Key).ToList();

            if (!IsRoot)
            {
                foreach (var leaf in Parent.Leafs)
                    result.AddRange(leaf.Elements);
            }

            return result.Clean();
        }

        private int GetHashCode(int element) => element % _maxSize;

        public override string ToString()
        {
            return $"[Level: {Level}] [Key: {Key}]";
        }
    }
}
