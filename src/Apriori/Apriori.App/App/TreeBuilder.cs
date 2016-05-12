using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Apriori.App.Structure;
using static System.Console;

namespace Apriori.App.App
{
    class TreeBuilder
    {
        private readonly int _maxSize;

        private HashTree[] _forest;

        //Sampling
        private const int K = 24;

        public TreeBuilder(int maxSize)
        {
            _maxSize = maxSize;
        }


        public HashTree Build()
        {
            var reader = new FileReader();

            int[][] dataSet = reader.Read("retail.dat.txt");
            //int[][] dataSet = { new[] {1, 2, 3, 4, 5, 6}, new[] {4 ,5}, new[] {5, 6}, new[] {1}, new[] {1, 5}};


            var jobs = new Task[K];
            _forest = new HashTree[K];

            for (int i = 0; i < K; i++)
            {
                int j = i;
                var job = new Task(() => Job(dataSet, j));
                jobs[i] = job;
                job.Start();
            }

            Task.WaitAll(jobs);


            HashTree hashTree = _forest[0];

            foreach (var tree in _forest.Skip(1))
            {
                hashTree.Merge(tree);
            }


            WriteLine("Tree properly saved.");
            return hashTree;
        }

        private void Job(int[][] dataSet, int k)
        {
            var tree = new HashTree(_maxSize);

            var stopWatch = new Stopwatch();
            for (int i = 0; i < dataSet.Length; i++)
            {
                if (i % K != k) continue;

                stopWatch.Start();
                tree.Add(dataSet[i]);
                stopWatch.Stop();
                Print(i + 1, dataSet.Length, dataSet[i].Length, stopWatch.Elapsed.TotalSeconds);
                stopWatch.Restart();
            }

            _forest[k] = tree;

            tree.Save($"hash-{_maxSize}-{k}.tree");
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
