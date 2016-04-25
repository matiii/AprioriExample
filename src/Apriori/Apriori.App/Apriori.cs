using System.Collections.Generic;
using System.Linq;
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
            Leaf source = node.Leafs.First(x => x.Exist(new[] {product}));

            foreach (var children in node)
                items.AddRange(
                    children.Value.Leafs.Where(x => x.Elements[0] == product)
                        .Select(
                            x => new FrequentItem(x.Elements[0], x.Elements[1], x.Attempts, _tree.NumberTransactions, source.Attempts)));

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
                    var right = sets.First(x => x.Exist(left.Elements.Take(i-1).ToArray()));

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

    }
}
