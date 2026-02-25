using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GroupManager : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public NavigationGraph graph;

    [Header("Path Settings")]
    public float repathInterval = 0.5f;
    public float repathDistanceThreshold = 1.5f;

    [Header("Debug")]
    public List<Node> currentPath = new List<Node>();

    private List<IGroupMember> members = new List<IGroupMember>();
    private float repathTimer;
    private Vector3 lastPlayerPos;

    void Start()
    {
        lastPlayerPos = player.position;
    }

    void Update()
    {
        if (player == null || graph == null) return;

        repathTimer += Time.deltaTime;

        bool playerMovedEnough =
            Vector3.Distance(player.position, lastPlayerPos) > repathDistanceThreshold;

        if (repathTimer >= repathInterval || playerMovedEnough)
        {
            ComputePath();
            repathTimer = 0f;
            lastPlayerPos = player.position;
        }
    }

    void ComputePath()
    {
        if (members.Count == 0)
        {
            Debug.LogWarning("[ComputePath] Operation cancelled: Members count is 0.");
            return;
        }
        
        IGroupMember leader = members.OrderBy(m => Vector3.Distance(m.GetPosition(), player.position)).First();

        Debug.Log($"[ComputePath] Group leader: {leader.GetPosition().ToString("F3")}, player position: {player.position.ToString("F3")}");

        Node startNode = graph.GetClosestNode(leader.GetPosition());
        Node targetNode = graph.GetClosestNode(player.position);

        if (startNode == null || targetNode == null)
        {
            Debug.LogError($"[ComputePath] Failed: {(startNode == null ? "StartNode" : "TargetNode")} is null. Check if the position is outside the Graph bounds.");
            return;
        }

        Debug.Log($"[ComputePath] Pathfinding from Node: {startNode.position} to Node: {targetNode.position}");

        currentPath = AStar.FindPath(startNode, targetNode);

        if (currentPath != null && currentPath.Count > 0)
        {
            Debug.Log($"[ComputePath] Success! Path found. Waypoints: {currentPath.Count}");
        }
        else
        {
            Debug.LogWarning("[ComputePath] Done, but no valid path was found between nodes.");
        }
    }

    Vector3 GetGroupCenter()
    {
        Vector3 sum = Vector3.zero;

        foreach (var m in members)
            sum += m.GetPosition();

        return sum / members.Count;
    }

    // Spider 自动注册
    public void Register(IGroupMember spider)
    {
        if (!members.Contains(spider))
            members.Add(spider);
    }

    public void Unregister(IGroupMember spider)
    {
        if (members.Contains(spider))
            members.Remove(spider);
    }

    public List<Node> GetCurrentPath()
    {
        return currentPath;
    }
    public List<IGroupMember> GetAllMembers() => members;
}