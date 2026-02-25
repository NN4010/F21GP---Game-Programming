using UnityEngine;
using System.Collections.Generic;

public class CrowdManager : MonoBehaviour
{
    public GameObject spiderPrefab; // 将你的蜘蛛预制体拖到这里
    public int numberOfSpiders = 12; // 课程要求至少 12 个 
    public float spawnRadius = 5f;   // 生成半径

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
            // 1. 保持高度：使用你预定的 y 坐标，或者直接从 Prefab 获取
            float fixedY = spiderPrefab.transform.position.y; // 比如 0.2
            
            Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPos = new Vector3(randomCircle.x, fixedY, randomCircle.y) + transform.position;

            // 2. 保留旋转：使用 spiderPrefab.transform.rotation 而不是 Quaternion.identity
            // 这样生成的蜘蛛就会保留那个关键的 X: -90 旋转
            GameObject newSpider = Instantiate(spiderPrefab, spawnPos, spiderPrefab.transform.rotation);
            
            newSpider.GetComponent<spider>().set_id(i);
            // 3. 物理初始化（防止刚体导致掉落）
            Rigidbody rb = newSpider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // 如果你不想让它有任何“掉落”动作，可以先将它设为 Kinematic，位置确定后再切回
                rb.interpolation = RigidbodyInterpolation.Interpolate;
            }
        }
    }
}