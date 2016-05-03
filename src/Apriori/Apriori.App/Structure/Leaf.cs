using System;
using System.Collections.Generic;
using System.Linq;

namespace Apriori.App.Structure
{
    [Serializable]
    class Leaf
    {
        public int[] Elements { get; }
        public int Attempts { get; private set; }
        public double? Support { get; private set; }

        public Leaf(IEnumerable<int> source)
        {
            if (source == null)
                throw new InvalidCastException("Leaf must have value.");

            Elements = source.ToArray();
            Attempts = 1;
        }

        public void Inc() => Attempts++;

        public bool Exist(int[] vector)
        {
            if (vector == null || Elements.Length != vector.Length) return false;
            if (Elements.Where((t, i) => t != vector[i]).Any()) return false;
            return true;
        }

        public bool HasSupport(double support, int numberTransactions)
        {
            if (Support.HasValue)
                return Support.Value >= support;

            Support = Attempts/(double)numberTransactions;

            return HasSupport(support, numberTransactions);
        }

        public override int GetHashCode() => Elements.Aggregate(13, (current, element) => current*7 + element.GetHashCode());

        public override bool Equals(object obj)
        {
            var leaf = obj as Leaf;
            return leaf != null && Exist(leaf.Elements);
        }

        public override string ToString() => $"Elements: {GetElements()} | Attempts: {Attempts} | Support: {Support ?? -1.0}";


        private string GetElements()
        {
            if (Elements == null) return "";
            string[] e = Elements.Select(x => x.ToString()).ToArray();
            return e.Aggregate((a, b) => a + " " + b);
        }
    }
}
