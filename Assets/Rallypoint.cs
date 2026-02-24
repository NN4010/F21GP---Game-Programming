using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RallyPoint : MonoBehaviour
{
    public int rallyValue = 1;
    public AudioClip collectSound;

    private bool collected = false;

    void Update()
    {
        // Simple rotation for visual effect
        transform.Rotate(0, 100 * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !collected)
        {
            collected = true;

            // Add rally points & food capacity
            GameManager.instance.AddRallyPoint(rallyValue);

            // Play sound
            if (collectSound != null)
            {
                AudioSource.PlayClipAtPoint(collectSound, transform.position);
            }

            // Optional: small scale effect before destroy
            Destroy(gameObject);
        }
    }
}
