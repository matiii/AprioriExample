using System;
using System.Collections.Generic;
using System.Linq;

namespace Apriori.App.Structure
{
    class Node : Dictionary<int, Node>
    {
        private readonly List<Leaf> _leafs = new List<Leaf>();
        private readonly HashSet<int> _uniqueValues = new HashSet<int>();

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

        public Node(Node parent, int[] elements, int maxSize) : this(maxSize)
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

        //TODO: optimize -> get only elements which contains in parent
        public void GenerateLeafs(int[] vector)
        {
            int[] items = GetNodeElements(vector);
            int[] items2 = GetParentElements(vector);

            //var elements = items.Union(items2).Clean();

            var que = new Queue<List<int>>();

            if (Parent.IsRoot)
            {
                foreach (var element in items)
                    que.Enqueue(new List<int> { element });
            }

            foreach (var element in items2)
                que.Enqueue(new List<int> { element });

            while (que.Count > 0)
            {
                var job = que.Dequeue();

                if (job.Count == Level)
                {
                    if (!items.Contains(job.Last())) continue;
                    //if (Parent.Leafs.Length > 0 && !Parent.Leafs.Any(x => x.Exist(job.Take(Level - 1).ToArray()))) continue;

                    var leaf = _leafs.FirstOrDefault(x => x.Exist(job.ToArray()));

                    if (leaf == null)
                        AddLeaf(new Leaf { Attempts = 1, Elements = job.ToArray() });
                    else
                        leaf.Attempts++;
                }
                else if (job.Count == Level - 1)
                {
                    foreach (var item in items.Where(x => x > job.Last()))
                    {
                        job.Add(item);
                        que.Enqueue(job);
                    }
                }
                else
                {
                    int index = Array.IndexOf(items2, job.Last()) + 1;

                    for (int i = index; i < items2.Length; i++)
                    {
                        job.Add(items2[i]);
                        que.Enqueue(job);
                    }
                }
            }
        }

        public bool ContainsElement(int element) => _uniqueValues.Contains(element);


        private void AddLeaf(Leaf leaf)
        {
            _leafs.Add(leaf);
            _uniqueValues.UnionWith(leaf.Elements);
        }

        private int[] GetNodeElements(int[] elements) => elements.Where(element => GetHashCode(element) == Key).ToArray();

        private int[] GetParentElements(int[] elements) => elements.Where(x => Parent.ContainsElement(x)).ToArray();

        private int GetHashCode(int element) => element % _maxSize;

        public override string ToString()
        {
            return $"[Level: {Level}] [Key: {Key}]";
        }
    }
}
