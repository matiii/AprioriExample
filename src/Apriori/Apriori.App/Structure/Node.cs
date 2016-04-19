using System.Collections.Generic;
using System.Linq;

namespace Apriori.App.Structure
{
    class Node : Dictionary<int, Node>
    {
        private readonly List<Leaf> _leafs = new List<Leaf>();
        private readonly List<int> _elements = new List<int>();

        private readonly int _maxSize;

        public Node Parent { get; }
        public int Level { get; }
        public int Key { get; }

        public Leaf[] Leafs => _leafs.ToArray();
        public int[] Elements => _elements.ToArray();
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

                int element = elements[current.Level];
                int key = GetHashCode(element);

                if (current.ContainsKey(key))
                {
                                                        
                }
                else
                {
                    current.Add(key, new Node(current, elements, _maxSize));
                }
            }

            
        }

        public void GenerateLeafs(int[] elements, bool filter = true)
        {
            int[] items = filter ? GetNodeElements(elements).ToArray() : elements;

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
                    int index = job.Count - 1;

                    for (int i = index; i < elements.Length; i++)
                        que.Enqueue(new List<int>(job) { elements[i] });
                }
            }
        }

        private IEnumerable<int> GetNodeElements(int[] elements) => elements.Where(element => GetHashCode(element) == Key);

        private int GetHashCode(int element) => element % _maxSize;

        public override string ToString()
        {
            return $"[Level: {Level}] [Key: {Key}]";
        }
    }
}
