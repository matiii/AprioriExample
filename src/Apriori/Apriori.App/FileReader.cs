using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Apriori.App
{
    class FileReader
    {
        public int[][] Read(string path)
        {
            var list = new List<int[]>();

            using (var sr = new StreamReader(path))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    list.Add(line.Split(' ').Where(x => x.Length > 0).Select(x => Int32.Parse(x)).ToArray());
                }
            }

            return list.ToArray();
        }
    }
}
