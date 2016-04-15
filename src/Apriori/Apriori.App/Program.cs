using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apriori.App.Structure;
using static System.Console;

namespace Apriori.App
{
    class Program
    {
        static void Main(string[] args)
        {
            Print(null, null);

            try
            {
                Do();
            }
            catch (Exception ex)
            {
                long memory = GC.GetTotalMemory(true);
                WriteLine($"Błąd: {ex.Message}");   
            }

            WriteLine("Press any key to exit...");
            ReadKey();
        }

        private static void Print(int? current, int? toProcess)
        {
            Clear();
            WriteLine("Projekt 1 | ADD");
            WriteLine("Mateusz Mazurek s12657 | Filip Stybel s89..");
            WriteLine("Repo: https://github.com/matiii/AprioriExample");

            if (current.HasValue && toProcess.HasValue)
            {
                WriteLine();
                WriteLine();
                WriteLine($"Process: {current.Value}/{toProcess.Value}");
            }
        }


        private static void Do()
        {
            var reader = new FileReader();

            int[][] dataSet = reader.Read("retail.dat.txt");

            var tree = new HashTree(4);

            for (int i = 0; i < dataSet.Length; i++)
            {
                Print(i+1, dataSet.Length);
                tree.Add(dataSet[i]);
            }
            


            //Node[] nodes = tree.GetNodesByDeep(3).ToArray();
            //Node[][] variation = nodes[0].GetAllVariations(3).ToArray();

            var apriori = new Apriori(tree, 3, 3, 0.6);
            var sets = apriori.GetFrequentSets();
            var items = apriori.GetFrequentItems(4);
            var rules = apriori.GetAssociationRules();
        }
    }
}
