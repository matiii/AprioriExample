using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Apriori.App.Structure
{
    [Serializable]
    class HashTree: ISerializable
    {
        private int _numberOfTransactions;
        private readonly int _maxSize;
        private readonly Node _nodes;

        public Node Root => _nodes;

        public int NumberTransactions => _numberOfTransactions;
        public int MaxSize => _maxSize;

        public HashTree(int maxSize)
        {
            _maxSize = maxSize;
            _nodes = new Node(maxSize);
        }

        public void Add(int[] input)
        {
            _numberOfTransactions++;
            _nodes.Add(input);
        }

        public void Merge(HashTree tree)
        {
            _numberOfTransactions += tree.NumberTransactions;
            var que = new Queue<Node>();
            que.Enqueue(tree.Root);

            while (que.Count > 0)
            {
                Node node = que.Dequeue();

                if (!node.IsRoot)
                    Map(node);

                foreach (var n in node)
                    que.Enqueue(n.Value);
            }
        }

        public void SetParents()
        {
            var que = new Queue<Node>();
            que.Enqueue(_nodes);

            while (que.Count > 0)
            {
                Node node = que.Dequeue();

                foreach (var n in node)
                {
                    que.Enqueue(n.Value);
                    n.Value.Parent = node;
                }
            }
        }

        private void Map(Node node)
        {
            var path = new List<int>();
            Node current = node;

            while (!current.IsRoot)
            {
                path.Add(current.Key);
                current = current.Parent;
            }

            path.Reverse();
            int[] result = path.ToArray();

            current = Root;

            foreach (var p in result)
                current = current[p];

            Reduce(node, current);
        }

        private void Reduce(Node source, Node destination)
        {
            foreach (var leaf in source.Leafs)
            {
                var dest = destination.GetLeaf(leaf.GetHashCode());

                if (dest == null)
                {
                    destination.AddLeaf(leaf);
                }
                else
                {
                    for (int i = 0; i < leaf.Attempts; i++)
                        dest.Inc();
                }
            }
        }

        public void Save(string path)
        {
            using (var file = File.Create(path))
            {
                var serializer = new BinaryFormatter();
                serializer.Serialize(file, this);
            }
        }

        public static HashTree Load(string path)
        {
            if (path == null || !File.Exists(path))
                throw new InvalidOperationException($"File: {path} doesn't exist.");
            if (!Path.GetExtension(path).Equals(".tree", StringComparison.InvariantCultureIgnoreCase))
                throw new InvalidOperationException($"File: {path} must has .tree extension.");

            using (var file = File.OpenRead(path))
            {
                var deserializer = new BinaryFormatter();
                file.Seek(0, SeekOrigin.Begin);
                return deserializer.Deserialize(file) as HashTree;
            }
        }


        protected HashTree(SerializationInfo info, StreamingContext context)
        {
            _numberOfTransactions = (int) info.GetValue(nameof(_numberOfTransactions), typeof(int));
            _maxSize = (int) info.GetValue(nameof(_maxSize), typeof(int));
            _nodes = (Node) info.GetValue(nameof(_nodes), typeof(Node));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(_numberOfTransactions), _numberOfTransactions);
            info.AddValue(nameof(_maxSize), _maxSize);
            info.AddValue(nameof(_nodes), _nodes);
        }
    }
}
