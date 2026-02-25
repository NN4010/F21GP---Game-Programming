using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector3 position;

    // Adjacent edges (Walk / Jump)
    public List<Edge> edges = new List<Edge>();

    // ===== A* Data =====

    public float gCost;   // From the starting point to the present
    public float hCost;   // Current to target
    public float fCost => gCost + hCost;

    public Node parent;   // Used for path backtracking

    public Node(Vector3 pos)
    {
        position = pos;
    }

    public void ResetPathData()
    {
        gCost = 0;
        hCost = 0;
        parent = null;
    }
}