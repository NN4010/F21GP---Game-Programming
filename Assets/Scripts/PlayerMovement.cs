using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;
    public float jumpForce = 5f;
    public float pickupRange = 2f;

    [Header("Physics & Grounding")]
    public LayerMask groundMask;
    public float groundCheckDistance = 0.3f;

    private Rigidbody rb;
    private bool isGrounded;
    private Vector3 inputMove;
    public Transform cam;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        inputMove = (camForward * moveZ + camRight * moveX).normalized;

        Vector3 origin = transform.position + Vector3.up * 0.1f;
        isGrounded = Physics.Raycast(origin, Vector3.down, groundCheckDistance, groundMask);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space Pressed");
            Debug.Log("Grounded: " + isGrounded);

            Debug.Log("position: " + transform.position + "Vector3.up"+ Vector3.up + "Vector3.down"+Vector3.down);
            if(isGrounded)
            {
                var v = rb.velocity;
                v.y = 0;
                rb.velocity = v;

                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryPickup();
        }
    }

    void FixedUpdate()
    {
        Vector3 velocity = inputMove * speed;
        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);

        if (inputMove != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(inputMove);
        }
    }

    void TryPickup()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, pickupRange);
        foreach (Collider hit in hits)
        {
            RallyPoint rp = hit.GetComponent<RallyPoint>();
            if (rp != null && rp.TakeSnack())
            {
                GameManager.instance.AddSnack(1);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 origin = transform.position + Vector3.up * 0.1f;
        Gizmos.DrawLine(origin, origin + Vector3.down * (groundCheckDistance + 0.1f));
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy touch ");
            GameManager.instance.TakeDamage();
        }
    }
}