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

        public Apriori(HashTree tree, int minSup, int maxSize)
        {
            _tree = tree;
            _minSup = minSup;
            _maxSize = maxSize;
        }

        public FrequentSets[] GetFrequentSets()
        {
            var list = new List<FrequentSets>();

            for (int i = 1; i <= _maxSize; i++)
            {
                foreach (var node in _tree.GetNodesByDeep(i))
                {
                    IEnumerable<Node[]> variations = node.GetAllVariations(i);

                    foreach (var variation in variations.Where(x => x.Length > 0 && x.Last().Attempts >= _minSup))
                    {
                        list.Add(new FrequentSets
                        {
                            Attempts = variation.Last().Attempts,
                            Set = variation.Select(x => x.Key).ToArray()
                        });
                    }

                }
            }

            return list.ToArray();
        }

        //TODO: check is it working
        public FrequentItem[] GetFrequentItems(int key)
        {
            var items = new List<FrequentItem>();

            //1 condition -> get all from element key
            if (_tree.Root.ContainsKey(key))
            {
                items.AddRange(
                    _tree
                    .Root[key]
                    .Where(x => x.Value.Attempts >= _minSup)
                    .Select(x => new FrequentItem
                    {
                        Source = key,
                        Item = x.Value.Key,
                        Attempts = x.Value.Attempts
                    }));
            }

            //2 condition -> get items from key less than source key
            foreach (var node in _tree.Root)
            {
                if (node.Key == key)
                    break;

                if (!node.Value.ContainsKey(key))
                    continue;

                items.Add(new FrequentItem { Source = key, Item = node.Key, Attempts = node.Value[key].Attempts });
            }

            return items.OrderByDescending(x => x.Attempts).ToArray();
        }

    }
}
