using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (members.Count == 0) return;

        Vector3 groupCenter = GetGroupCenter();

        Node startNode = graph.GetClosestNode(groupCenter);
        Node targetNode = graph.GetClosestNode(player.position);

        if (startNode == null || targetNode == null)
            return;

        currentPath = AStar.FindPath(startNode, targetNode);
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
}