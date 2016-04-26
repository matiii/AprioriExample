using System;
using static System.Console;

namespace Apriori.App.App
{
    class Manager
    {

        private readonly string[] _input;

        public Manager(string[] input)
        {
            _input = input;
            Reponse();
        }

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

        private void Reponse()
        {

            if (_input.Length == 2 && _input[0] == "buildtree")
            {
                int maxSize;

                if (Int32.TryParse(_input[1], out maxSize) && maxSize > 1)
                    BuildTree(maxSize);
            }
            else if (_input.Length == 2 && _input[0] == "frequentSets")
            {
                double minSup;

                if (Double.TryParse(_input[1], out minSup))
                    FrequentSets(minSup);
            }
            else if (_input.Length == 2 && _input[0] == "frequentProducts")
            {
                int product;

                if (Int32.TryParse(_input[1], out product))
                    FrequentProducts(product);
            }
            else if (_input.Length == 3 && _input[0] == "associationRules")
            {
                double minSup, minConf;

                if (Double.TryParse(_input[1], out minSup) && Double.TryParse(_input[2], out minConf))
                    AssociationRules(minSup, minConf);
            }
            else if (_input.Length == 4 && _input[0] == "associationRules")
            {
                double minSup, minConf;
                int maxSize;

                if (Double.TryParse(_input[1], out minSup) && Double.TryParse(_input[2], out minConf) && Int32.TryParse(_input[3], out maxSize))
                    AssociationRules(minSup, minConf, maxSize);
            }
            else if (_input.Length == 2 && _input[0] == "associationProducts")
            {
                int product;

                if (Int32.TryParse(_input[1], out product))
                    AssociationProducts(product);
            }
            else
                ViewHelp();
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

        }

        private void BuildTree(int maxSize)
        {
            var builder = new TreeBuilder(maxSize);
            builder.Build();
        }

        private void ViewHelp()
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
                    WriteLine();
                    WriteLine();
                    WriteLine("Examples:");
                    WriteLine();
                    WriteLine("buildtree 3 <- its build hash tree where the largest leaf will be have 3 elements");
                    WriteLine("frequentSets 0.6 <- return all frequnts sets which all have support more or equal than 0.6");

                }
            );
        }
    }
}
