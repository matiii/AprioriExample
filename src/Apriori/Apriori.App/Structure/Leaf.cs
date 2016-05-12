using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Apriori.App.Structure
{
    [Serializable]
    class Leaf: ISerializable
    {
        private readonly int _hashCode;

        public int[] Elements { get; }
        public int Attempts { get; private set; }
        public double? Support { get; private set; }

        public Leaf(IEnumerable<int> source)
        {
            if (source == null)
                throw new InvalidCastException("Leaf must have value.");

            Elements = source.ToArray();
            Attempts = 1;
            string key = String.Join("_", Elements);
            _hashCode = key.GetHashCode();
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

            Support = Attempts / (double) numberTransactions;

            return HasSupport(support, numberTransactions);
        }

        public override int GetHashCode() => _hashCode;

        protected Leaf(SerializationInfo info, StreamingContext context)
        {
            _hashCode = (int) info.GetValue(nameof(_hashCode), typeof(int));
            Elements = (int[]) info.GetValue(nameof(Elements), typeof(int[]));
            Attempts = (int) info.GetValue(nameof(Attempts), typeof(int));
            Support = (double?) info.GetValue(nameof(Support), typeof(double?));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(_hashCode), _hashCode);
            info.AddValue(nameof(Elements), Elements);
            info.AddValue(nameof(Attempts), Attempts);
            info.AddValue(nameof(Support), Support);
        }

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
