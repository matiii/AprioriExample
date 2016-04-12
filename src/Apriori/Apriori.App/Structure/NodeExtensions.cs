using System.Collections.Generic;
using System.Linq;

namespace Apriori.App.Structure
{
    static class NodeExtensions
    {
        /// <summary>
        /// Get all variations of path in connected nodes
        /// </summary>
        /// <param name="node">Parent node</param>
        /// <param name="depth"></param>
        /// <returns></returns>
        public static IEnumerable<Node[]> GetAllVariations(this Node node, int depth)
        {
            var que = new Queue<List<Node>>();
            que.Enqueue(new[] { node }.ToList());

            while (que.Count > 0)
            {
                List<Node> path = que.Dequeue();

                if (path.Count == depth)
                    yield return path.ToArray();
                else
                {
                    foreach (var last in path.Last())
                        que.Enqueue(new List<Node>(path) {last.Value});
                }
            }
        }
    }
}
