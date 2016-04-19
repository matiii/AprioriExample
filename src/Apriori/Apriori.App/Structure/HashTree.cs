using System;
using System.Collections.Generic;
using System.Linq;

namespace Apriori.App.Structure
{
    class HashTree
    {
        private readonly int _maxSize;
        //private readonly Node _nodes = new Node(new[] {0}, 0);

        //public Node Root => _nodes;

        //public HashTree(int maxSize)
        //{
        //    _maxSize = maxSize;
        //}

        //public void Add(int[] input)
        //{
        //    foreach (var i in input)
        //        _nodes.Add(new[] {i});

        //    Set(input);
        //}

        //private void Set(int[] input)
        //{
        //    int max = input.Length > _maxSize ? _maxSize : input.Length;

        //    if (max < 2)
        //        return;

        //    var que = new Queue<Task>();
        //    que.Enqueue(new Task { Node = _nodes, Size = 1});

        //    while (que.Count > 0 && que.First().Size < max)
        //    {
        //        Task level = que.Dequeue();

        //        foreach (var node in level.Node)
        //        {
        //            int index = Array.IndexOf(input, node.Key);

        //            if (index < 0)
        //                continue;

        //            for (int i = index + 1; i < input.Length; i++)
        //                //node.Value.Add();

        //            que.Enqueue(new Task {Node = node.Value, Size = level.Size + 1});
        //        }

        //    }
            
        //}
        
    }
}
