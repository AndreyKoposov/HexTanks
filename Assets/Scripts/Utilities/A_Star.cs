using System;
using System.Collections.Generic;

public class A_Star
{
    private class Node : IComparable<Node>
    {
        public Node parent;
        public VectorHex position;
        public float G;
        public float H;
        public float F => G + H;

        public int CompareTo(Node other)
        {
            return F.CompareTo(other.F);
        }
    }

    public static List<VectorHex> FindShortestPath(VectorHex from, VectorHex to, int maxIterations = 1000)
    {
        if (from == to)
            throw new HexTankException("From equals To");

        var openSet = new SortedSet<Node>();
        var closedSet = new HashSet<VectorHex>();
        var nodeCache = new Dictionary<VectorHex, Node>();

        var startNode = new Node()
        {
            position = from,
            G = 0,
            H = H(from ,to)
        };

        openSet.Add(startNode);
        nodeCache[from] = startNode;

        int iterations = 0;

        while (openSet.Count > 0 && iterations < maxIterations)
        {
            iterations++;

            var nextNode = openSet.Min;
            openSet.Remove(nextNode);

            if (nextNode.position == to)
                return ReconstructPath(nextNode);

            closedSet.Add(nextNode.position);

            foreach (var neighbourPos in nextNode.position.Neighbours)
            {
                if (closedSet.Contains(neighbourPos)) continue;
                if (Game.Grid[neighbourPos].IsObstacle) continue;

                if (!nodeCache.TryGetValue(neighbourPos, out var neighbourNode))
                {
                    neighbourNode = new Node() { position = neighbourPos };
                    nodeCache[neighbourPos] = neighbourNode;
                }

                if (nextNode.G < neighbourNode.G || !openSet.Contains(neighbourNode))
                {
                    neighbourNode.parent = nextNode;
                    neighbourNode.G = nextNode.G;
                    neighbourNode.H = H(neighbourPos, to);

                    if (openSet.Contains(neighbourNode))
                        openSet.Remove(neighbourNode);

                    openSet.Add(neighbourNode);
                }
            }
        }

        if (iterations > maxIterations)
            throw new HexTankException("Iterations limit reached");
        else
            throw new HexTankException("There is no path");
    }

    private static float H(VectorHex from, VectorHex to)
    {
        return (Game.Grid[to].transform.position - Game.Grid[from].transform.position).magnitude;
    }

    private static List<VectorHex> ReconstructPath(Node endNode)
    {
        List<VectorHex> path = new();
        var nextNode = endNode;

        while (nextNode != null)
        {
            path.Add(nextNode.position);
            nextNode = nextNode.parent;
        }

        path.Reverse();

        return path;
    }
}
