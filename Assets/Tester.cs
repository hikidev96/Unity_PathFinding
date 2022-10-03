using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Yemin.PathFinding
{
    public class Tester : MonoBehaviour
    {
        private PathFinding pathFinding = new PathFinding();

        [SerializeField] private Vector2Int dest;
        [SerializeField] private float moveSpeed = 1.0f;

        private List<Node> path;

        private void Update()
        {
            path = pathFinding.FindPath((int)this.transform.position.x, (int)this.transform.position.y, dest.x, dest.y);

            if (path == null || Vector2.Distance(this.transform.position, dest) < 0.1f) return;

            var nextPos = new Vector2Int(path[1].X, path[1].Y);
            this.transform.Translate((nextPos - new Vector2(this.transform.position.x, this.transform.position.y)).normalized * Time.deltaTime * moveSpeed, Space.World);
        }

        private void OnDrawGizmos()
        {
            for (int i = 0; i < pathFinding.Grid.Height; ++i)
            {
                for (int j = 0; j < pathFinding.Grid.Width; ++j)
                {
                    Gizmos.DrawWireCube(new Vector2(pathFinding.Grid[i, j].X, pathFinding.Grid[i, j].Y), new Vector2(pathFinding.Grid.NodeSize, pathFinding.Grid.NodeSize));
                    Handles.Label(new Vector2(pathFinding.Grid[i, j].X, pathFinding.Grid[i, j].Y), $"{i},{j}");

                    if (path != null)
                    {
                        for (int k = 0; k < path.Count; ++k)
                        {
                            Gizmos.DrawSphere(new Vector2(path[k].X, path[k].Y), 0.5f);
                        }
                    }
                    else
                    {
                        Debug.Log("Path is null");
                    }
                }
            }
        }
    }
}