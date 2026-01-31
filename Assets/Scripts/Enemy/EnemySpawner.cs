using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float spawnRadius = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefab == null) return;
        // 1 vị trí ngẫu nhiên trong bán kính spawnRadius 
        Vector2 randomPos =  Random.insideUnitSphere * spawnRadius;
        //2 trục Y của 2D sẽ là trục Z trong 3D
        // cộng thêm vị trí của player để nó luôn sinh ra quanh player
        Vector3 spawnPos = new Vector3(randomPos.x, 0f, randomPos.y) + PlayerMovement.instance.position;
        //3 tạo quái
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
