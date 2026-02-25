using System.Collections.Generic;
using UnityEngine;

public class NavigationGraph : MonoBehaviour
{
    [Header("Graph Settings")]
    public float walkConnectionDistance = 3f;
    public float jumpConnectionDistance = 5f;
    public float maxJumpHeight = 2f;

    public List<Node> nodes = new List<Node>();

    void Awake()
    {
        BuildGraph();
    }

    void BuildGraph()
    {
        nodes.Clear();

        // 找到场景所有 NodePoint
        NodePoint[] points = FindObjectsOfType<NodePoint>();

        // 创建 Node 数据
        foreach (var p in points)
        {
            nodes.Add(new Node(p.transform.position));
        }

        // 建立连接
        for (int i = 0; i < nodes.Count; i++)
        {
            for (int j = i + 1; j < nodes.Count; j++)
            {
                TryConnect(nodes[i], nodes[j]);
            }
        }
    }

    void TryConnect(Node a, Node b)
    {
        float horizontalDist = Vector3.Distance(
            new Vector3(a.position.x, 0, a.position.z),
            new Vector3(b.position.x, 0, b.position.z)
        );

        float heightDiff = Mathf.Abs(a.position.y - b.position.y);

        // Walk 连接
        if (horizontalDist <= walkConnectionDistance && heightDiff < 0.5f)
        {
            float cost = horizontalDist;
            a.edges.Add(new Edge(b, cost, EdgeType.Walk));
            b.edges.Add(new Edge(a, cost, EdgeType.Walk));
        }

        // Jump 连接
        else if (horizontalDist <= jumpConnectionDistance && heightDiff <= maxJumpHeight)
        {
            float cost = horizontalDist + heightDiff;
            a.edges.Add(new Edge(b, cost, EdgeType.Jump));
            b.edges.Add(new Edge(a, cost, EdgeType.Jump));
        }
    }

    public Node GetClosestNode(Vector3 position)
    {
        Node closest = null;
        float minDist = Mathf.Infinity;

        foreach (var node in nodes)
        {
            float dist = Vector3.Distance(position, node.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = node;
            }
        }

        return closest;
    }

    // 可视化
    void OnDrawGizmos()
    {
        if (nodes == null) return;

        Gizmos.color = Color.yellow;

        foreach (var node in nodes)
        {
            Gizmos.DrawSphere(node.position, 0.2f);

            foreach (var edge in node.edges)
            {
                if (edge.type == EdgeType.Walk)
                    Gizmos.color = Color.green;
                else
                    Gizmos.color = Color.red;

                Gizmos.DrawLine(node.position, edge.target.position);
            }
        }
    }
}