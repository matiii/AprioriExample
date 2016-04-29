using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;

namespace Apriori.App.Structure
{
    [Serializable]
    class HashTree
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
                var dest = destination.Leafs.FirstOrDefault(x => x.Exist(leaf.Elements));

                if (dest == null)
                {

                }
                else
                {
                    
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
                return deserializer.Deserialize(file) as HashTree;
            }
        }

    }
}
