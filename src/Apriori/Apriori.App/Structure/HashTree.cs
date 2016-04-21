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
            
        }

    }
}
