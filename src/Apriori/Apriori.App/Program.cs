using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apriori.App.App;
using Apriori.App.Structure;
using static System.Console;

namespace Apriori.App
{
    class Program
    {
        static void Main(string[] args)
        {
            //Print(null, null);

            try
            {
                var manager = new Manager(args);
            }
            catch (Exception ex)
            {
                WriteLine($"Błąd: {ex.Message}");   
            }

            WriteLine();
            WriteLine("Press any key to exit...");
            ReadKey();
        }

        private static void Print(int? current, int? toProcess, int length = 0, double time = 0)
        {
            Clear();
            WriteLine("Projekt 1 | ADD");
            WriteLine("Mateusz Mazurek s12657 | Filip Stybel s89..");
            WriteLine("Repo: https://github.com/matiii/AprioriExample");

            if (current.HasValue && toProcess.HasValue)
            {
                WriteLine();
                WriteLine();
                WriteLine($"Element of {length} length was processed by {time.ToString("F")} sec.");
                WriteLine($"Process: {current.Value}/{toProcess.Value}");
            }
        }


        private static void Do()
        {
            //var reader = new FileReader();

            //int[][] dataSet = reader.Read("retail.dat.txt");

            ////int[][] dataSet = {new[] {1, 2, 3, 4}, new[] {1, 2, 3, 4}};


            //var tree = new HashTree(3);

            //var stopWatch = new Stopwatch();
            //for (int i = 0; i < dataSet.Length; i++)
            //{
            //    stopWatch.Start();
            //    tree.Add(dataSet[i]);
            //    stopWatch.Stop();
            //    Print(i + 1, dataSet.Length, dataSet[i].Length, stopWatch.Elapsed.TotalSeconds);
            //    stopWatch.Restart();
            //}


            //tree.Save("hash.tree");

            HashTree load = HashTree.Load("hash.tree");

            //Node[] nodes = tree.GetNodesByDeep(3).ToArray();
            //Node[][] variation = nodes[0].GetAllVariations(3).ToArray();

            //var apriori = new Apriori(tree, 3, 3, 0.6);
            //var sets = apriori.GetFrequentSets();
            //var items = apriori.GetFrequentItems(4);
            //var rules = apriori.GetAssociationRules();
        }

        
        

    }
}
