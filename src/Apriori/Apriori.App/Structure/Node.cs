using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Apriori.App.Structure
{
    [Serializable]
    class Node : Dictionary<int, Node>, ISerializable
    {
        private readonly Dictionary<int, Leaf> _leafs = new Dictionary<int, Leaf>();

        private readonly int _maxSize;

        public Node Parent { get; }
        public int Level { get; }
        public int Key { get; }

        public Dictionary<int, Leaf>.ValueCollection Leafs => _leafs.Values;

        public bool IsRoot => Parent == null;

        protected Node(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            _leafs = (Dictionary<int, Leaf>) info.GetValue(nameof(_leafs), typeof(Dictionary<int, Leaf>));
            _maxSize = (int) info.GetValue(nameof(_maxSize), typeof(int));
            Parent = (Node) info.GetValue(nameof(Parent), typeof(Node));
            Level = (int) info.GetValue(nameof(Level), typeof(int));
            Key = (int) info.GetValue(nameof(Key), typeof(int));
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(_leafs), _leafs);
            info.AddValue(nameof(_maxSize), _maxSize);
            info.AddValue(nameof(Parent), Parent);
            info.AddValue(nameof(Level), Level);
            info.AddValue(nameof(Key), Key);
        }

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

        public void GenerateLeafs(int[] vector)
        {
            int[] items = GetNodeElements(vector);

            var que = new Queue<List<int>>();

            if (Parent.IsRoot)
            {
                foreach (var element in items)
                    que.Enqueue(new List<int> { element });
            }
            else
            {
                foreach (var leaf in Parent.Leafs.Where(x => x.Elements.All(e => vector.Contains(e))))
                    que.Enqueue(new List<int>(leaf.Elements));
            }

            while (que.Count > 0)
            {
                var job = que.Dequeue();

                if (job.Count == Level)
                {
                    var leaf = new Leaf(job);
                    int key = leaf.GetHashCode();

                    if (_leafs.ContainsKey(key))
                        _leafs[key].Inc();
                    else
                        AddLeaf(leaf);
                }
                else if (job.Count == Level - 1)
                {
                    foreach (var item in items.Where(x => x > job.Last()))
                        que.Enqueue(new List<int>(job) {item});
                }
                else
                    throw new InvalidOperationException("Error");
            }
        }


        public void AddLeaf(Leaf leaf)
        {
            int key = leaf.GetHashCode();

            if (_leafs.Count == Int32.MaxValue)
            {
                double avg = _leafs.Average(x => x.Value.Attempts);
                
                foreach (var toDelete in _leafs.Where(x => x.Value.Attempts < avg && x.Value.GetHashCode() != key).Select(x => x.Value).ToList())
                {
                    _leafs.Remove(toDelete.GetHashCode());
                }
            }

            _leafs.Add(key, leaf);
        }

        private int[] GetNodeElements(int[] elements) => elements.Where(element => GetHashCode(element) == Key).ToArray();

        public int GetHashCode(int element) => element % _maxSize;

        public override string ToString()
        {
            return $"[Level: {Level}] [Key: {Key}]";
        }
    }
}
