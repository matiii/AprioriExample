using System;
using System.IO;
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

        //TODO
        public void Merge(HashTree tree)
        {
            throw new NotImplementedException();
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
