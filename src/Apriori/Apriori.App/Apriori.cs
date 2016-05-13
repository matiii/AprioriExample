using System;
using System.Collections.Generic;
using System.Linq;
using Apriori.App.App;
using Apriori.App.Structure;

namespace Apriori.App
{
    class Apriori
    {
        private readonly HashTree _tree;

        public Apriori(HashTree tree)
        {
            _tree = tree;
        }

        public int NumberOfUniqProducts() => _tree.Root.Sum(node => node.Value.Leafs.Count);

        public Leaf[] GetFrequentSets(double minSupport, bool maxSize = false, int size = 0)
        {
            var list = new List<Leaf>();

            var que = new Queue<Node>();
            que.Enqueue(_tree.Root);

            while (que.Count > 0)
            {
                Node node = que.Dequeue();

                var frequentLeafs = node.Leafs.Where(x => x.HasSupport(minSupport, _tree.NumberTransactions)).ToArray();
                list.AddRange(frequentLeafs);

                bool can = node.IsRoot || frequentLeafs.Length > 0;

                if (maxSize ? node.Level < size && can : can)
                {
                    foreach (var children in node)
                        que.Enqueue(children.Value);
                }
            }

            return list.ToArray();
        }


        public Leaf[] GetFrequentSets(double minSupport, int size)
        {
            var list = new List<Leaf>();

            var que = new Queue<Node>();
            que.Enqueue(_tree.Root);

            while (que.Count > 0)
            {
                Node node = que.Dequeue();

                if (node.Level < size)
                {
                    foreach (var children in node)
                        que.Enqueue(children.Value);
                    continue;
                }

                var frequentLeafs = node.Leafs.Where(x => x.HasSupport(minSupport, _tree.NumberTransactions)).ToArray();
                list.AddRange(frequentLeafs);
            }

            return list.ToArray();
        }

        public FrequentItem[] GetFrequentItems(int product)
        {
            var items = new List<FrequentItem>();
            int key = _tree.Root.GetHashCode(product);

            Node node = _tree.Root[key];
            Leaf source = node.GetLeaf(new Leaf(new[] {product}).GetHashCode());

            foreach (var nodes in _tree.Root.Where(x => x.Key < key))
            {
                Node node2 = nodes.Value[key];
                items.AddRange(
                        node2.Leafs.Where(x => x.Elements[1] == product)
                            .Select(
                                child =>
                                    new FrequentItem(child.Elements[1], child.Elements[0], child.Attempts,
                                        _tree.NumberTransactions, source.Attempts)));
                    
            }

            foreach (var children in node)
                items.AddRange(
                    children.Value.Leafs.Where(x => x.Elements[0] == product)
                        .Select(
                            child =>
                                new FrequentItem(child.Elements[0], child.Elements[1], child.Attempts,
                                    _tree.NumberTransactions, source.Attempts)));
            

            return items.OrderByDescending(x => x.Attempts).ToArray();
        }

        public FrequentItem[] GetFrequentItems(int product, double confidence)
            => GetFrequentItems(product).Where(x => x.Confidence >= confidence).ToArray();

        public AssociationRule[] GetAssociationRules(double minSup, double minConf)
        {
            Leaf[] sets = GetFrequentSets(minSup);

            AssociationRule[] items = GetAssociationRules(sets, minConf, _tree.MaxSize).ToArray();

            return items;
        }

        public AssociationRule[] GetAssociationRules(double minSup, double minConf, int maxSize)
        {
            Leaf[] sets = GetFrequentSets(minSup, true, maxSize);

            AssociationRule[] items = GetAssociationRules(sets, minConf, maxSize).ToArray();

            return items;
        }


        private IEnumerable<AssociationRule> GetAssociationRules(Leaf[] sets, double minConf, int maxSize)
        {
            for (int i = 2; i <= maxSize; i++)
            {
                foreach (var left in sets.Where(_ => _.Elements.Length == i))
                {
                    Leaf right = GetLeaf(left.Elements.Take(i-1).ToArray());

                    if (right == null)
                    {
                        Manager.HeadMsg(() => Console.WriteLine($"GetAssociationRules {left.Elements.Select(x => x.ToString()).Aggregate((a, b) => a + " " + b)}"), () => {Console.WriteLine("Brak elementu w drzewie.");});
                        continue;
                    }

                    double confidence = left.Support.Value/right.Support.Value;

                    if (confidence >= minConf)
                        yield return new AssociationRule
                        {
                            Confidence = confidence,
                            Left = left.Elements,
                            Right = right.Elements
                        };
                }
            }
        }

        private Leaf GetLeaf(int[] vector)
        {
            Node node = _tree.Root;

            for (int i = 0; i < vector.Length; i++)
            {
                int key = node.GetHashCode(vector[i]);
                node = node[key];
            }

            return node.GetLeaf(new Leaf(vector).GetHashCode());
        }
    }
}
