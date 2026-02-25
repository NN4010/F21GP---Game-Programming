using System.Collections.Generic;
using UnityEngine;

public class Node
{
    // 世界坐标
    public Vector3 position;

    // 邻接边（Walk / Jump）
    public List<Edge> edges = new List<Edge>();

    // ===== A* 用的数据 =====

    public float gCost;   // 起点到当前
    public float hCost;   // 当前到目标
    public float fCost => gCost + hCost;

    public Node parent;   // 用于回溯路径

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