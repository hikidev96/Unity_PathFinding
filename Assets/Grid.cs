using System.Collections.Generic;
using UnityEngine;

namespace Yemin.PathFinding
{
    public class Grid
    {
        private int width;
        private int height;
        private float nodeSize;
        private List<List<Node>> grid = new List<List<Node>>();

        public int Width => width;
        public int Height => height;
        public float NodeSize => nodeSize;

        public Node this[int y, int x]
        {
            get { return grid[y][x]; }
        }

        public Grid(int width, int height, float nodeSize)
        {
            this.width = width;
            this.height = height;
            this.nodeSize = nodeSize;

            for (int i = 0; i < height; i++)
            {
                var horizontalNodes = new List<Node>(); 

                for (int j = 0; j < width; j++)
                {
                    var node = new Node();
                    node.X = j;
                    node.Y = i;

                    horizontalNodes.Add(node);
                }

                grid.Add(horizontalNodes);  
            }
        }

        public Node GetNode(int x, int y)
        {
            return grid[y][x];
        }
    }
}