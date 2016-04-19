using System.Collections.Generic;
using System.Linq;

namespace Apriori.App.Structure
{
    static class HashTreeExtensions
    {
        /// <summary>
        /// Returns all nodes where parent is Root and has depth moreOrEqual
        /// </summary>
        /// <param name="tree">Source</param>
        /// <param name="moreOrEqual">Length of depth</param>
        /// <returns></returns>
        //public static IEnumerable<Node> GetNodesByDeep(this HashTree tree, int moreOrEqual)
        //{
        //    return from node in tree.Root where HasDepth(node, moreOrEqual) select node.Value;
        //}

        private static bool HasDepth(KeyValuePair<int, Node> node, int moreOrEqual)
        {
            int current = 1;
            var que = new Queue<Task>();
            que.Enqueue(new Task { Node = node.Value, Size = current });

            while (que.Count > 0)
            {
                Task task = que.Dequeue();
                current = task.Size;

                if (current >= moreOrEqual)
                    break;

                foreach (var taskNode in task.Node)
                    que.Enqueue(new Task {Node = taskNode.Value, Size = current + 1});
            }

            return current >= moreOrEqual;
        }
    }
}
