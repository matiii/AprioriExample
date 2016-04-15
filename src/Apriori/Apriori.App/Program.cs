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
            WriteLine("Projekt 1 | ADD");
            WriteLine("Mateusz Mazurek s12657 | Filip Stybel s89..");
            WriteLine("Repo: https://github.com/matiii/AprioriExample");

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

        private static void Do()
        {
            var reader = new FileReader();

            int[][] dataSet = reader.Read("retail.dat.txt");

            var tree = new HashTree(4);

            foreach (var data in dataSet)
                tree.Add(data);


            //Node[] nodes = tree.GetNodesByDeep(3).ToArray();
            //Node[][] variation = nodes[0].GetAllVariations(3).ToArray();

            var apriori = new Apriori(tree, 3, 3, 0.6);
            var sets = apriori.GetFrequentSets();
            var items = apriori.GetFrequentItems(4);
            var rules = apriori.GetAssociationRules();
        }
    }
}
