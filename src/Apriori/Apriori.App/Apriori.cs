using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        //TODO
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
                
                if (node.IsRoot || frequentLeafs.Length > 0)
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

            foreach (var children in node)
                items.AddRange(
                    children.Value.Leafs.Where(x => x.Elements[0] == product)
                        .Select(
                            x => new FrequentItem {Attempts = x.Attempts, Item = x.Elements[1], Source = x.Elements[0]}));

            return items.OrderByDescending(x => x.Attempts).ToArray();
        }

        public AssociationRule[] GetAssociationRules(double minSup, double minConf)
        {
            var items = new List<AssociationRule>();

            Leaf[] sets = GetFrequentSets(minSup);

            for (int i = 2; i <= _tree.MaxSize; i++)
            {
                foreach (var left in sets.Where(_ => _.Elements.Length == i))
                {
                    var right = sets.First(x => x.Exist(left.Elements.Take(i-1).ToArray()));

                    double confidence = left.Support.Value/right.Support.Value;

                    if (confidence >= minConf)
                        items.Add(new AssociationRule
                        {
                            Confidence = confidence,
                            Left = left.Elements,
                            Right = right.Elements
                        });
                }
            }

            return items.ToArray();
        }

        //TODO
        public AssociationRule[] GetAssociationRules(double minSup, double minConf, int maxSize)
        {
            var items = new List<AssociationRule>();

            Leaf[] sets = GetFrequentSets(minSup, maxSize);

            for (int i = 2; i <= _tree.MaxSize; i++)
            {
                foreach (var left in sets.Where(_ => _.Elements.Length == i))
                {
                    var right = sets.First(x => x.Exist(left.Elements.Take(i-1).ToArray()));

                    double confidence = left.Support.Value/right.Support.Value;

                    if (confidence >= minConf)
                        items.Add(new AssociationRule
                        {
                            Confidence = confidence,
                            Left = left.Elements,
                            Right = right.Elements
                        });
                }
            }

            return items.ToArray();
        }

        //private double GetConfidence(AssociationRule rule)
        //{
        //    Node current = _tree.Root;

        //    foreach (var l in rule.Left)
        //        current = current[l];

        //    double leftSupport = 0;//current.Attempts;

        //    current = _tree.Root;

        //    foreach (var r in rule.Right)
        //        current = current[r];

        //    double rightSupport = 0; //current.Attempts;

        //    rule.Confidence = leftSupport/rightSupport;
        //    return rule.Confidence;
        //}

        //private AssociationRule[] GetAllVariationsOfSet(FrequentSets set)
        //{
        //    int left = 1;
        //    int right = set.Set.Length - 1;

        //    var rules = new List<AssociationRule>();

        //    while (right != 0)
        //    {
        //        rules.Add(new AssociationRule
        //        {
        //            Left = set.Set.Take(left).ToArray(),
        //            Right = set.Set.Skip(left).Take(right).ToArray()
        //        });

        //        left++;
        //        right--;
        //    }

        //    int length = rules.Count;

        //    for (int i = 0; i < length; i++)
        //    {
        //        rules.Add(new AssociationRule
        //        {
        //            Left = rules[i].Right,
        //            Right = rules[i].Left
        //        });
        //    }

        //    return rules.ToArray();
        //}
    }
}
