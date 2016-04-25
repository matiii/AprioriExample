namespace Apriori.App
{
    class FrequentItem
    {
        public int Source { get;}
        public int Item { get;}
        public int Attempts { get; }
        public double Support { get; }
        public double Confidence { get; }

        public FrequentItem(int source, int item, int attempts, int transactionCount, int sourceCount)
        {
            Source = source;
            Item = item;
            Attempts = attempts;

            Support = Attempts/(double)transactionCount;
            Confidence = Attempts/(double)sourceCount;
        }


        public override string ToString() => $"{Source} => {Item} Support: {Support} Confidence: {Confidence}";
    }
}
