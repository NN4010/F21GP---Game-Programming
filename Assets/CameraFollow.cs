using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;              // Drag your player here
    public Vector3 offset = new Vector3(0, 1, 2); // Behind & above
    public float followSpeed = 0.125f;    // Smooth movement
    public float rotationSpeed = 5f;      // Smooth rotation

    private void LateUpdate()
    {
        if (player == null) return;

        // --- Position the camera behind the player relative to their rotation ---
        Vector3 desiredPosition = player.position + player.rotation * offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed);

        // --- Rotate the camera to match the player's forward direction ---
        Quaternion targetRotation = Quaternion.Euler(player.eulerAngles.x, player.eulerAngles.y, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
