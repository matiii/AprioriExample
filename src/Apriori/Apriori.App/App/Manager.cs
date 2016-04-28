﻿using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Apriori.App.Structure;
using static System.Console;

namespace Apriori.App.App
{
    class Manager
    {

        public HashTree Tree { get; set; }
        public string TreeName { get; set; }

        public string DirectoryPath
            => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "AprioriApp");


        public static void HeadMsg(Action operation, Action body)
        {
            Clear();
            WriteLine("Projekt 1 | ADD");
            operation?.Invoke();
            WriteLine("Mateusz Mazurek s12657 | Filip Stybel s8292");
            WriteLine("Repo: https://github.com/matiii/AprioriExample");
            WriteLine();
            WriteLine();
            body?.Invoke();
        }


        public void Start()
        {
            string path = GetTheLargestTree();
            TreeName = Path.GetFileNameWithoutExtension(path);
            HeadMsg(() => { WriteLine($"Inicjalizacja - {TreeName}"); }, () => { WriteLine("Trwa wczytywanie drzewa."); });

            HandleDirectory();

            Tree = HashTree.Load(path);
        }

        private void HandleDirectory()
        {
            if (!Directory.Exists(DirectoryPath))
            {
                Directory.CreateDirectory(DirectoryPath);

                string path = Path.Combine(DirectoryPath, "readme.txt");

                File.WriteAllText(path, "Apriori app by Mateusz Mazurek and Filip Stybel.");
            }
        }

        public void Reponse(string[] input)
        {

            if (input.Length == 2 && input[0] == "buildtree")
            {
                int maxSize;

                if (Int32.TryParse(input[1], out maxSize) && maxSize > 1)
                    BuildTree(maxSize);
            }
            else if (input.Length == 2 && input[0] == "frequentSets")
            {
                double minSup;

                if (Double.TryParse(input[1], NumberStyles.Number, CultureInfo.InvariantCulture, out minSup))
                    FrequentSets(minSup);
            }
            else if (input.Length == 3 && input[0] == "frequentSets")
            {
                double minSup;
                int maxSize;

                if (Double.TryParse(input[1], NumberStyles.Number, CultureInfo.InvariantCulture, out minSup) && Int32.TryParse(input[2], out maxSize))
                    FrequentSets(minSup, maxSize);
            }
            else if (input.Length == 2 && input[0] == "frequentProducts")
            {
                int product;

                if (Int32.TryParse(input[1], out product))
                    FrequentProducts(product);
            }
            else if (input.Length == 3 && input[0] == "associationRules")
            {
                double minSup, minConf;

                if (Double.TryParse(input[1], out minSup) && Double.TryParse(input[2], out minConf))
                    AssociationRules(minSup, minConf);
            }
            else if (input.Length == 4 && input[0] == "associationRules")
            {
                double minSup, minConf;
                int maxSize;

                if (Double.TryParse(input[1], out minSup) && Double.TryParse(input[2], out minConf) && Int32.TryParse(input[3], out maxSize))
                    AssociationRules(minSup, minConf, maxSize);
            }
            else if (input.Length == 2 && input[0] == "associationProducts")
            {
                int product;

                if (Int32.TryParse(input[1], out product))
                    AssociationProducts(product);
            }
        }

        private void AssociationProducts(int product)
        {
            throw new NotImplementedException();
        }

        private void AssociationRules(double minSup, double minConf, int maxSize)
        {
            throw new NotImplementedException();
        }

        private void AssociationRules(double minSup, double minConf)
        {
            throw new NotImplementedException();
        }

        private void FrequentProducts(int product)
        {
            throw new NotImplementedException();
        }

        private void FrequentSets(double minSup)
        {
            var apriori = new Apriori(Tree);

            Leaf[] leafs = apriori.GetFrequentSets(minSup);

            FrequentSets(leafs, $"FrequentSets {minSup} Find elements: {leafs.Length}", $"{TreeName}-frequentsets-{minSup.ToString().Replace(".", "_").Replace(",", "_")}.txt");
        }

        private void FrequentSets(double minSup, int maxSize)
        {
            var apriori = new Apriori(Tree);

            Leaf[] leafs = apriori.GetFrequentSets(minSup, maxSize);

            FrequentSets(leafs, $"FrequentSets {minSup} {maxSize} Find elements: {leafs.Length}", $"{TreeName}-frequentsets-{minSup.ToString().Replace(".", "_").Replace(",", "_")}-{maxSize}.txt");
        }


        private void FrequentSets(Leaf[] leafs, string msg, string filename)
        {
            var sb = new StringBuilder();
            sb.AppendLine(msg);

            foreach (var leaf in leafs.OrderByDescending(x => x.Support))
                sb.AppendLine(leaf.ToString());

            string path = Path.Combine(DirectoryPath, filename);

            File.WriteAllText(path, sb.ToString());
        }

        private string GetTheLargestTree()
        {
            string currentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string path = Directory.GetFiles(currentDirectory, "*.tree").OrderByDescending(x => x).FirstOrDefault();

            if (path == null)
                throw new Exception("Any tree was generated.");

            return path;
        }


        private void BuildTree(int maxSize)
        {
            var builder = new TreeBuilder(maxSize);
            builder.Build();
        }

        public void ViewHelp()
        {
            HeadMsg(() =>
            {
                WriteLine("Help");
            },
                () =>
                {
                    WriteLine("Avaiable commands:");
                    WriteLine("* buildtree [maxSize: int]");
                    WriteLine("* frequentSets [minSup: double]");
                    WriteLine("* frequentProducts [product: int]");
                    WriteLine("* associationRules [minSup: double] [minConf: double]");
                    WriteLine("* associationRules [minSup: double] [minConf: double] [maxSize: int]");
                    WriteLine("* associationProducts [product: int]");
                    WriteLine("* quit");
                    WriteLine();
                    WriteLine();
                    WriteLine("Examples:");
                    WriteLine();
                    WriteLine("buildtree 3 <- its build hash tree which the largest leaf will be have 3 elements");
                    WriteLine("frequentSets 0.6 <- return all frequnts sets which all have support more or equal than 0.6");

                }
            );
        }
    }
}
