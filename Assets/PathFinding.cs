using System.Collections.Generic;
using UnityEngine;

namespace Yemin.PathFinding
{
    public class PathFinding
    {
        private const int MOVE_STRAIGHT_COST = 10;
        private const int MOVE_DIAGONAL_COST = 14;

        private Grid grid = new Grid(10, 10, 1.0f);
        private List<Node> openNodes;
        private List<Node> closeNodes;

        public Grid Grid => grid;

        public List<Node> FindPath(int startX, int startY, int endX, int endY)
        {
            if (IsAvaliableIndex(startX, startY) == false) return null;
            if (IsAvaliableIndex(endX, endY) == false) return null;

            var startNode = grid.GetNode(startX, startY);
            var endNode = grid.GetNode(endX, endY);

            openNodes = new List<Node>() { startNode };
            closeNodes = new List<Node>();

            for (int i = 0; i < grid.Width; ++i)
            {
                for (int j = 0; j < grid.Height; ++j)
                {
                    Node node = grid.GetNode(j, i);
                    node.GCost = int.MaxValue;
                    node.CalculateFCost();
                    node.ParentNode = null;
                }
            }

            startNode.GCost = 0;
            startNode.HCost = CalculateDistanceCost(startNode, endNode);
            startNode.CalculateFCost();

            while (openNodes.Count > 0)
            {
                Node currentNode = GetLowerstFCostNode(openNodes);

                if (currentNode == endNode)
                {
                    return CalculatePath(endNode);
                }

                openNodes.Remove(currentNode);
                closeNodes.Add(currentNode);

                foreach (Node neighbourNode in GetNeighbourNodes(currentNode))
                {
                    if (closeNodes.Contains(neighbourNode)) continue;
                    if (neighbourNode.IsWalkable == false)
                    {
                        closeNodes.Add(neighbourNode);
                        continue;
                    }

                    int tentativeGCost = currentNode.GCost + CalculateDistanceCost(currentNode, neighbourNode);
                    if (tentativeGCost < neighbourNode.GCost)
                    {
                        neighbourNode.ParentNode = currentNode;
                        neighbourNode.GCost = tentativeGCost;
                        neighbourNode.HCost = CalculateDistanceCost(neighbourNode, endNode);
                        neighbourNode.CalculateFCost();

                        if (openNodes.Contains(neighbourNode) == false)
                        {
                            openNodes.Add(neighbourNode);
                        }
                    }
                }
            }

            return null;
        }

        private List<Node> GetNeighbourNodes(Node currentNode)
        {
            var result = new List<Node>();

            if (currentNode.X - 1 >= 0)
            {
                result.Add(grid.GetNode(currentNode.X - 1, currentNode.Y));
                if (currentNode.Y - 1 >= 0) result.Add(grid.GetNode(currentNode.X - 1, currentNode.Y - 1));
                if (currentNode.Y + 1 < grid.Height) result.Add(grid.GetNode(currentNode.X - 1, currentNode.Y + 1));
            }
            if (currentNode.X + 1 < grid.Width)
            {
                result.Add(grid.GetNode(currentNode.X + 1, currentNode.Y));
                if (currentNode.Y - 1 >= 0) result.Add(grid.GetNode(currentNode.X + 1, currentNode.Y - 1));
                if (currentNode.Y + 1 < grid.Height) result.Add(grid.GetNode(currentNode.X + 1, currentNode.Y + 1));
            }
            if (currentNode.Y - 1 >= 0) result.Add(grid.GetNode(currentNode.X, currentNode.Y - 1));
            if (currentNode.Y + 1 < grid.Height) result.Add(grid.GetNode(currentNode.X, currentNode.Y + 1));

            return result;
        }

        private List<Node> CalculatePath(Node endNode)
        {
            var result = new List<Node>();
            result.Add(endNode);
            var currentNode = endNode;
            while (currentNode.ParentNode != null)
            {
                result.Add(currentNode.ParentNode);
                currentNode = currentNode.ParentNode;
            }
            result.Reverse();
            return result;
        }

        private int CalculateDistanceCost(Node a, Node b)
        {
            int xDistance = Mathf.Abs(a.X - b.X);
            int yDistance = Mathf.Abs(a.Y - b.Y);
            int remaining = Mathf.Abs(xDistance - yDistance);

            return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
        }

        private Node GetLowerstFCostNode(List<Node> nodes)
        {
            var lowerstFCostNode = nodes[0];

            for (int i = 1; i < nodes.Count; ++i)
            {
                if (nodes[i].FCost < lowerstFCostNode.FCost)
                {
                    lowerstFCostNode = nodes[i];
                }
            }

            return lowerstFCostNode;
        }

        private bool IsAvaliableIndex(int x, int y)
        {
            if (x < 0 || x >= grid.Width)
            {
                return false;
            }

            if (y < 0 || y >= grid.Height)
            {
                return false;
            }

            return true;
        }
    }
}