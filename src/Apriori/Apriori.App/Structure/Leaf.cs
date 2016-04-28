using System;
using System.Linq;

namespace Apriori.App.Structure
{
    [Serializable]
    class Leaf
    {
        public int[] Elements { get; set; }
        public int Attempts { get; set; }
        public double? Support { get; private set; }

        public bool Exist(int[] vector)
        {
            if (Elements == null || vector == null || Elements.Length != vector.Length) return false;
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

        public override int GetHashCode()
        {
            int hash = 13;

            if (Elements == null) return hash;

            foreach (var element in Elements)
                hash *= 7 + element.GetHashCode();

            return hash;
        }

        public override bool Equals(object obj)
        {
            var leaf = obj as Leaf;
            if (leaf == null || !Exist(leaf.Elements)) return false;
            return true;
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
