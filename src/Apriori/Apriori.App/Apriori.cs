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
        private readonly int _minSup;
        private readonly int _maxSize;
        private readonly double _minConf;

        public Apriori(HashTree tree, int minSup, int maxSize, double minConf)
        {
            _tree = tree;
            _minSup = minSup;
            _maxSize = maxSize;
            _minConf = minConf;
        }

        public FrequentSets[] GetFrequentSets()
        {
            var list = new List<FrequentSets>();

            //for (int i = 1; i <= _maxSize; i++)
            //{
            //    foreach (var node in _tree.GetNodesByDeep(i))
            //    {
            //        //IEnumerable<Node[]> variations = node.GetAllVariations(i);

            //        //foreach (var variation in variations.Where(x => x.Length > 0 && x.Last().Attempts >= _minSup))
            //        //{
            //        //    list.Add(new FrequentSets
            //        //    {
            //        //        Attempts = variation.Last().Attempts,
            //        //        Set = variation.Select(x => x.Key).ToArray()
            //        //    });
            //        //}

            //    }
            //}

            return list.ToArray();
        }

        public FrequentItem[] GetFrequentItems(int key)
        {
            var items = new List<FrequentItem>();

            //1 condition -> get all from element key
            //if (_tree.Root.ContainsKey(key))
            //{
            //    items.AddRange(
            //        _tree
            //        .Root[key]
            //        .Where(x => x.Value.Attempts >= _minSup)
            //        .Select(x => new FrequentItem
            //        {
            //            Source = key,
            //            Item = x.Value.Key,
            //            Attempts = x.Value.Attempts
            //        }));
            //}

            //2 condition -> get items from key less than source key
            //foreach (var node in _tree.Root)
            //{
            //    if (node.Key == key)
            //        break;

            //    if (!node.Value.ContainsKey(key) || node.Value[key].Attempts < _minSup)
            //        continue;

            //    items.Add(new FrequentItem { Source = key, Item = node.Key, Attempts = node.Value[key].Attempts });
            //}

            return items.OrderByDescending(x => x.Attempts).ToArray();
        }

        //public AssociationRule[] GetAssociationRules()
        //{
        //    var items = new List<AssociationRule>();

        //    FrequentSets[] sets = GetFrequentSets();

        //    foreach (var set in sets.Where(x => x.Depth > 1))
        //    {
        //        AssociationRule[] rules = GetAllVariationsOfSet(set);
        //        items.AddRange(rules.Where(x => GetConfidence(x) >= _minConf));
        //    }

        //    return items.ToArray();
        //}

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
