using System;

namespace Apriori.App.Structure
{
    class HashTree
    {
        private readonly Node _nodes;

        public Node Root => _nodes;

        public HashTree(int maxSize)
        {
            _nodes = new Node(maxSize);
        }

        public void Add(int[] input)
        {
            _nodes.Add(input);
        }

        //TODO
        public void Merge(HashTree tree)
        {
            throw new NotImplementedException();
        }

        //TODO
        public void Save(string path)
        {
            throw new NotImplementedException();
        }

        //TODO
        public static HashTree Load(string path)
        {
            throw new NotImplementedException();
        }

    }
}
