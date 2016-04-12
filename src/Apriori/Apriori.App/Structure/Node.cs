using System.Collections.Generic;

namespace Apriori.App.Structure
{
    class Node: Dictionary<int, Node>
    {
        public int Attempts { get; set; }
        public int Key { get; }

        public Node(int key)
        {
            Key = key;
            Attempts++;
        }

        public void Add(int key)
        {
            if (!ContainsKey(key))
                Add(key, new Node(key));
            else
                this[key].Attempts++;
        }

        public override string ToString()
        {
            return $"[Key: {Key}] [Attempts: {Attempts}]";
        }
    }
}
