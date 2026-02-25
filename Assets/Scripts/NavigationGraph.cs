using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class NavigationGraph : MonoBehaviour
{
    public float nodeSpacing = 1.5f; // 节点间距
    public Vector2 gridSize = new Vector2(20, 20);
    public List<Node> allNodes = new List<Node>();
    public static NavigationGraph Instance;

    public float minRebuildInterval = 0.5f;
    private float lastRebuildTime = -999f;
    private bool graphDirty = false;
    void Awake() { 
        Debug.Log($"[NavigationGraph] Awake");
        Instance = this;
        GenerateGraph(); 
        }

    void Update()
    {
        if (!graphDirty) return;

        if (Time.time - lastRebuildTime < minRebuildInterval)
            return;

        GenerateGraph();

        graphDirty = false;
        lastRebuildTime = Time.time;
    }

    public void GenerateGraph()
    {
        allNodes.Clear();
        Debug.Log($"[Graph] Starting generation at {transform.position}. Grid: {gridSize}");

        int attemptCount = 0;
        for (float x = -gridSize.x/2; x < gridSize.x/2; x += nodeSpacing) {
            for (float z = -gridSize.y/2; z < gridSize.y/2; z += nodeSpacing) {
                attemptCount++;
                Vector3 pos = transform.position + new Vector3(x, 0.5f, z); // 稍微抬高一点 Y

                // 增加搜索半径到 2.0f 确保能抓到地面
                if (NavMesh.SamplePosition(pos, out NavMeshHit hit, 2.0f, NavMesh.AllAreas)) {
                    allNodes.Add(new Node(hit.position));
                }
            }
        }

        Debug.Log($"[Graph] Attempted to sample {attemptCount} points.");
        Debug.Log($"[Graph] Successfully created {allNodes.Count} nodes.");

        if (allNodes.Count == 0) {
            Debug.LogError("!!! ALLNODES IS ZERO !!! 1. Check if NavMesh is Baked. 2. Check if object is near the floor.");
        }
        // 2. 连线：使用 Raycast 避开墙壁
        foreach (Node a in allNodes) {
            foreach (Node b in allNodes) {
                float dist = Vector3.Distance(a.position, b.position);
                if (dist > 0 && dist <= nodeSpacing * 1.5f) {
                    if (!NavMesh.Raycast(a.position, b.position, out NavMeshHit hit, NavMesh.AllAreas))
                        a.edges.Add(new Edge(b, dist, EdgeType.Walk));
                }
            }
        }
    }

    public Node GetClosestNode(Vector3 pos) {
        Node best = null; float minDist = float.MaxValue;
        foreach(var n in allNodes) {
            float d = Vector3.Distance(pos, n.position);
            if(d < minDist) { minDist = d; best = n; }
        }
        return best;
    }

    public void MarkDirty()
    {
        graphDirty = true;
    }
}