using System.Diagnostics;
using System.Linq;
using Apriori.App.Structure;
using static System.Console;

namespace Apriori.App.App
{
    class TreeBuilder
    {
        private readonly int _maxSize;

        public TreeBuilder(int maxSize)
        {
            _maxSize = maxSize;
        }


        public void Build()
        {
            var reader = new FileReader();

            int[][] dataSet = reader.Read("retail.dat.txt");
            //int[][] dataSet = { new[] {1, 2, 3, 4, 5, 6}, new[] {4 ,5}, new[] {5, 6}, new[] {1}, new[] {1, 5}};

            var tree = new HashTree(_maxSize);

            var stopWatch = new Stopwatch();
            for (int i = 0; i < dataSet.Length; i++)
            {
                stopWatch.Start();
                tree.Add(dataSet[i]);
                stopWatch.Stop();
                Print(i + 1, dataSet.Length, dataSet[i].Length, stopWatch.Elapsed.TotalSeconds);
                stopWatch.Restart();
            }


            tree.Save($"hash-{_maxSize}.tree");

            WriteLine("Tree properly saved.");
        }

        private void Print(int? current, int? toProcess, int length = 0, double time = 0)
        {
            Manager.HeadMsg(() =>
            {
                WriteLine($"Operation buildtree {_maxSize}");
            },
                () =>
                {
                    WriteLine();
                    WriteLine();
                    WriteLine($"Element of {length} length was processed by {time.ToString("F")} sec.");
                    WriteLine($"Process: {current.Value}/{toProcess.Value}");
                }
            );
        }
    }
}
