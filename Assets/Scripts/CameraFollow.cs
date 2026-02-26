using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    public float distance = 6f;
    public float height = 3f;
    public float rotationSpeed = 200f;

    private float currentYaw = 0f;

    void Start()
    {
        currentYaw = player.eulerAngles.y;
    }

    void LateUpdate()
    {
        if (!player) return;

        float mouseX = Input.GetAxis("Mouse X");
        currentYaw += mouseX * rotationSpeed * Time.deltaTime;

        
        Quaternion rotation = Quaternion.Euler(0, currentYaw, 0);
        Vector3 offset = rotation * new Vector3(0, height, -distance);

        transform.position = player.position + offset;

        // 看向玩家
        transform.LookAt(player.position + Vector3.up * 1.5f);
    }
}