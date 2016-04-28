using System;
using System.Linq;
using Apriori.App.App;
using static System.Console;

namespace Apriori.App
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Run();
            }
            catch (Exception ex)
            {
                WriteLine($"Błąd: {ex.Message}");   
            }

            WriteLine();
            WriteLine("Press any key to exit...");
            ReadKey();
        }

        private static void Run()
        {
            var manager = new Manager();
            manager.Start();

            bool noExit = true;

            while (noExit)
            {
                manager.ViewHelp();

                Write("> ");
                string input = ReadLine();
                string[] vector = input.Split(' ').Where(x => x.Length > 0).ToArray();

                manager.Reponse(vector);

                if (input == "quit")
                    noExit = false;
            }


        }
    }
}
