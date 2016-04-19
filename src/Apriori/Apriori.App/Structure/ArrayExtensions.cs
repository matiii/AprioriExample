using System.Linq;

namespace Apriori.App.Structure
{
    static class ArrayExtensions
    {
        public static int[] Clean(this int[] elements) => elements.Distinct().OrderBy(x => x).ToArray();
    }
}
