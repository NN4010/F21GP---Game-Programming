using UnityEngine;
using System.Collections.Generic;

public class CrowdManager : MonoBehaviour
{
    public GameObject spiderPrefab; 
    public int numberOfSpiders = 12; 
    public float spawnRadius = 5f; 

    private List<spider> allSpiders = new List<spider>();

    void Start()
    {
        SpawnGroup();
    }

    void SpawnGroup()
    {
        Debug.Log($"SpawnGroup: numberOfSpiders {numberOfSpiders}");
        for (int i = 0; i < numberOfSpiders; i++)
        {
            
            float fixedY = spiderPrefab.transform.position.y;
            
            Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPos = new Vector3(randomCircle.x, fixedY, randomCircle.y) + transform.position;

            GameObject newSpider = Instantiate(spiderPrefab, spawnPos, spiderPrefab.transform.rotation);
            
            newSpider.GetComponent<spider>().set_id(i);
            Rigidbody rb = newSpider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.interpolation = RigidbodyInterpolation.Interpolate;
            }
        }
    }
}