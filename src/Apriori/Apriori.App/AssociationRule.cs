using System.Linq;

namespace Apriori.App
{
    class AssociationRule
    {
        public int[] Left { get; set; }
        public int[] Right { get; set; }
        public double Confidence { get; set; }

        protected string[] LSide => Left.Select(x => x.ToString()).ToArray();
        protected string[] RSide => Right.Select(x => x.ToString()).ToArray();

        public override string ToString() => $"{LSide.Aggregate((a, b) => a + " " + b)} => {RSide.Aggregate((a,b) => a + " " + b)}: conf = {Confidence.ToString("F")}";
    }
}
