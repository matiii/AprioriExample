using System;

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

        }
    }
}
