using System;
using System.Collections.Generic;
using System.Linq;

namespace Apriori.App.Structure
{
    class HashTree
    {
        private readonly int _maxSize;
        private readonly Node _nodes = new Node(0);

        public HashTree(int maxSize)
        {
            _maxSize = maxSize;
        }

        public void Add(int[] input)
        {
            input = input.Distinct().OrderBy(x => x).ToArray();

            foreach (var i in input)
                _nodes.Add(i);

            Set(input);
        }

        private void Set(int[] input)
        {
            int max = input.Length > _maxSize ? _maxSize : input.Length;

            if (max < 2)
                return;

            var que = new Queue<Task>();
            que.Enqueue(new Task { Node = _nodes, Size = 1});

            while (que.Count > 0 && que.First().Size < max)
            {
                Task level = que.Dequeue();

                foreach (var node in level.Node)
                {
                    int index = Array.IndexOf(input, node.Key);

                    if (index < 0)
                        continue;

                    for (int i = index + 1; i < input.Length; i++)
                        node.Value.Add(input[i]);

                    que.Enqueue(new Task {Node = node.Value, Size = level.Size + 1});
                }

            }
            
        }


        private void _Set(int[] input)
        {
            int max = input.Length > _maxSize ? _maxSize : input.Length;

            if (max < 2)
                return;

            int current = 1;
            int index = 0;

            Node c = _nodes;
            Node level = c;

            while (current < max)
            {
                for (int i = current; i < input.Length; i++)
                {
                    int first = input[index];
                    int next = input[i];

                    if (level.ContainsKey(first))
                        level[first].Add(next);

                    if (i == input.Length - 1)
                    {
                        index++;
                        i = index;

                        if (i + 1 == input.Length)
                            break;
                    }
                }

                current++;

                if (current == max)
                {
                    current = 1;
                    level = c = c.First().Value;
                }
                else
                {
                    int key = input[current - 2];
                    level = c[key];
                }

                index = current - 1;


            }
        }
    }
}
