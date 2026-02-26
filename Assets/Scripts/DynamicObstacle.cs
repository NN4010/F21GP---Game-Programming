using UnityEngine;

public class DynamicObstacle : MonoBehaviour
{
    public float moveThreshold = 0.1f;

    private Vector3 lastPosition;
    private NavigationGraph graph;

    void Start()
    {
        lastPosition = transform.position;
        graph = FindObjectOfType<NavigationGraph>();
    }

    void Update()
    {
        
        float dist = Vector3.Distance(transform.position, lastPosition);
        Debug.Log($"DynamicObstacle update dist {dist}");
        if (dist > moveThreshold)
        {
            Debug.Log($"DynamicObstacle update dist1 {dist}");
            if (graph != null)
                Debug.Log($"DynamicObstacle update dist2 {dist}");
                graph.MarkDirty();   // 通知更新

            lastPosition = transform.position;
        }
    }
}