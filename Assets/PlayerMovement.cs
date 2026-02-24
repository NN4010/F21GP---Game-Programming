using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 720f;
    private CharacterController controller;
    private Vector3 velocity;
    private float gravity = -9.81f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Input
        float moveX = -Input.GetAxis("Horizontal"); // notice the minus sign
        float moveZ = -Input.GetAxis("Vertical");
        Vector3 move = new Vector3(moveX, 0, moveZ);

        // Movement
        if (move.magnitude > 0.1f)
        {
            // Rotate toward movement
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Apply movement in local forward direction
        controller.Move(move.normalized * speed * Time.deltaTime);

        // Apply gravity
        if (!controller.isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
        else
        {
            velocity.y = 0f; // reset when on ground
        }
    }
}
