using System.Collections.Generic;
using UnityEngine;

public static class AStar
{
    public static List<Node> FindPath(Node start, Node goal)
    {
        if (start == null || goal == null)
            return null;

        // 打开列表（待检查）
        List<Node> openSet = new List<Node>();

        // 关闭列表（已检查）
        HashSet<Node> closedSet = new HashSet<Node>();

        // 初始化
        ResetAllNodes(start);

        start.gCost = 0;
        start.hCost = Heuristic(start, goal);
        start.parent = null;

        openSet.Add(start);

        while (openSet.Count > 0)
        {
            Node current = GetLowestFCostNode(openSet);

            // 找到目标
            if (current == goal)
                return RetracePath(start, goal);

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (Edge edge in current.edges)
            {
                Node neighbor = edge.target;

                if (closedSet.Contains(neighbor))
                    continue;

                float tentativeG = current.gCost + edge.cost;

                if (!openSet.Contains(neighbor) || tentativeG < neighbor.gCost)
                {
                    neighbor.gCost = tentativeG;
                    neighbor.hCost = Heuristic(neighbor, goal);
                    neighbor.parent = current;

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }

        // 没找到路径
        return null;
    }

    static float Heuristic(Node a, Node b)
    {
        // 欧几里得距离
        return Vector3.Distance(a.position, b.position);
    }

    static Node GetLowestFCostNode(List<Node> nodes)
    {
        Node best = nodes[0];

        foreach (Node n in nodes)
        {
            if (n.fCost < best.fCost ||
               (n.fCost == best.fCost && n.hCost < best.hCost))
            {
                best = n;
            }
        }

        return best;
    }

    static List<Node> RetracePath(Node start, Node end)
    {
        List<Node> path = new List<Node>();
        Node current = end;

        while (current != start)
        {
            path.Add(current);
            current = current.parent;
        }

        path.Reverse();
        return path;
    }

    static void ResetAllNodes(Node start)
    {
        // 简化版本：只清理访问到的链条
        Node current = start;
        while (current != null)
        {
            current.gCost = 0;
            current.hCost = 0;
            current.parent = null;
            current = current.parent;
        }
    }
}