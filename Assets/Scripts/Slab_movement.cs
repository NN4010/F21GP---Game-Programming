using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMover : MonoBehaviour
{
    public Vector3 pointA;      // Start position
    public Vector3 pointB;      // End position
    public float speed = 2f;    // Movement speed

    private Vector3 target;     

    void Start()
    {
        target = pointB;  // First target
    }

    void Update()
    {
        // Move towards the target
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // Switch direction when reaching the target
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            target = (target == pointA) ? pointB : pointA;
        }
    }
}
