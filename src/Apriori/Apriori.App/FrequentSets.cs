namespace Apriori.App
{
    class FrequentSets
    {
        public int[] Set { get; set; }
        public int Attempts { get; set; }
        public int Depth => Set?.Length ?? 0;
    }
}
