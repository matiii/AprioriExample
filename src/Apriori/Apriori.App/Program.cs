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

            Do();

            WriteLine("Press any key to exit...");
            ReadKey();
        }

        private static void Do()
        {
            int[] fake = { 1, 2, 3, 4 };
            int[] fake1 = { 2, 3, 4 };
            //int[] fake2 = { 1, 2, 4, 5 };
            var tree = new HashTree(4);

            tree.Add(fake);
            tree.Add(fake1);
            //tree.Add(fake2);

            //Node[] nodes = tree.GetNodesByDeep(3).ToArray();
            //Node[][] variation = nodes[0].GetAllVariations(3).ToArray();

            var apriori = new Apriori(tree, 3, 3);
            var sets = apriori.GetFrequentSets();
        }
    }
}
