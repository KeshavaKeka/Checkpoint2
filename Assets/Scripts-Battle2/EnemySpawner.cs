using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab of the enemy to be spawned
    public int numberOfEnemies = 5; // Number of enemies to spawn
    public float minX = -10f, maxX = 10f, minZ = -10f, maxZ = 10f; // Boundaries for spawning
    public float minimumDistance = 5f; // Minimum distance between enemies
    public float navMeshSampleDistance = 1.0f; // Max distance for NavMesh sampling

    private List<Vector3> spawnPositions = new List<Vector3>();

    void Start()
    {
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        int spawnedCount = 0;

        while (spawnedCount < numberOfEnemies)
        {
            Vector3 randomPosition = GetRandomPosition();

            if (IsPositionValid(randomPosition))
            {
                Vector3 validPosition;
                if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, navMeshSampleDistance, NavMesh.AllAreas))
                {
                    validPosition = hit.position;
                    GameObject newEnemy = Instantiate(enemyPrefab, validPosition, Quaternion.identity);
                    NavMeshAgent agent = newEnemy.GetComponent<NavMeshAgent>();
                    if (agent != null && agent.isOnNavMesh)
                    {
                        spawnPositions.Add(validPosition);
                        spawnedCount++;
                    }
                    else
                    {
                        Destroy(newEnemy);
                    }
                }
            }
        }
    }

    Vector3 GetRandomPosition()
    {
        float randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);
        return new Vector3(randomX, 0, randomZ);
    }

    bool IsPositionValid(Vector3 position)
    {
        foreach (Vector3 spawnPosition in spawnPositions)
        {
            if (Vector3.Distance(position, spawnPosition) < minimumDistance)
            {
                return false;
            }
        }
        return true;
    }
}