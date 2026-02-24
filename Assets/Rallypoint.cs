using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RallyPoint : MonoBehaviour
{
    public int foodCapacity = 5;   // Total snacks here
    public GameObject catPrefab;   // Cat object
    public int catCount = 3;       // Cats near this point
    public float spawnRadius = 3f;

    void Start()
    {
        SpawnCats();
    }

    void SpawnCats()
    {
        for (int i = 0; i < catCount; i++)
        {
            Vector3 randomPos = transform.position +
                new Vector3(Random.Range(-spawnRadius, spawnRadius), 0,
                            Random.Range(-spawnRadius, spawnRadius));

            Instantiate(catPrefab, randomPos, Quaternion.identity);
        }
    }

    public bool TakeSnack()
    {
        if (foodCapacity > 0)
        {
            foodCapacity--;
            return true;
        }
        return false;
    }
}
