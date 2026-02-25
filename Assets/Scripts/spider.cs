using System.Collections.Generic;
using UnityEngine;

public class spider : MonoBehaviour, IGroupMember
{
    [Header("Movement Settings")]
    public float speed = 5f;
    public float separationDistance = 1.5f; // 蜘蛛间的安全距离
    public float separationWeight = 2.0f;   // 排斥力的强度

    private Rigidbody rb;
    private GroupManager manager;

    private int _id;
    public int ID => _id;

    void Start() {
        rb = GetComponent<Rigidbody>();
        rb.constraints =
            RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationZ |
            RigidbodyConstraints.FreezePositionY;
        manager = FindObjectOfType<GroupManager>();
        if (manager != null) manager.Register(this);
    }

    void FixedUpdate() {
        
        if (manager == null)
        {
            Debug.LogError($"spider manager is null");
            return;
        } 

        List<Node> path = manager.GetCurrentPath();
        Vector3 moveDirection = Vector3.zero;

        // 1. 遵循共享路径（主要驱动力）
        if (path != null && path.Count > 0) {
            moveDirection = (path[0].position - transform.position).normalized;
        }
        else
        {
            Debug.Log("spider path not found");
        }

        // 2. 个体微调：分离力（防止重叠）
        Vector3 separationForce = CalculateSeparation();
        moveDirection += separationForce * separationWeight;

        // 3. 应用物理移动
        moveDirection.y = 0;
        Debug.Log($"Move Direction: {moveDirection.ToString("F4")}, _id :{_id}");
        if (moveDirection != Vector3.zero) {
            moveDirection.Normalize();
            rb.velocity = new Vector3(moveDirection.x * speed, moveDirection.y * speed, moveDirection.z * speed);
            
            // 平滑转向目标
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * 10f);
            Debug.Log($"transform forward: {transform.forward.ToString("F4")}");
        }
    }

    Vector3 CalculateSeparation() {
        Vector3 force = Vector3.zero;
        var others = manager.GetAllMembers();

        foreach (var other in others) {
            if (other == (IGroupMember)this) continue;

            float dist = Vector3.Distance(transform.position, other.GetPosition());
            if (dist < separationDistance) {
                // 产生反方向推力
                force += (transform.position - other.GetPosition()).normalized / dist;
            }
        }
        return force;
    }

    public Vector3 GetPosition() => transform.position;
    private void OnDestroy() { if (manager != null) manager.Unregister(this); }

    public void set_id(int id) {
        _id = id;
    }
}