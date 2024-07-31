using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MazeGenerator : MonoBehaviour
{
    public NavMeshSurface surface;

    public int width = 13;
    public int height = 20;

    public GameObject[] wall;
    public GameObject enemy; // Add a reference to the enemy prefab

    private int[,] maze;
    private Stack<Vector2Int> stack = new Stack<Vector2Int>();

    // Start is called before the first frame update
    void Start()
    {
        GenerateMaze();
        SpawnEnemies(5,8f);
        surface = GameObject.Find("NavMesh").GetComponent<NavMeshSurface>();
        if (surface != null)
        {
            surface.BuildNavMesh();
        }
        else
        {
            Debug.Log("Surface not assigned");
        }
    }

    void GenerateMaze()
    {
        maze = new int[width, height];

        for (int i = 0; i < width; i+=2)
        {
            for (int j = 0; j < height; j+=2)
            {
                if (i == 0 || i == width - 2 || j == 0 || j == height - 2)
                {
                    int wid;
                    if((width/2)%2 == 1)
                    {
                        wid = width / 2 - 1;
                    }
                    else
                    {
                        wid = width / 2;
                    }
                    if(i != wid)
                    {
                        maze[i, j] = 1;
                        Vector3 position = new Vector3(i - width / 2f + 1f, wall[1].transform.position.y, j - height / 2f + 1f);
                        Instantiate(wall[1], position, Quaternion.identity);
                    }
                }
            }
        }
    }

    void SpawnEnemies(int count, float minDistance)
    {
        List<Vector2Int> openPositions = new List<Vector2Int>();
        List<Vector3> spawnedEnemies = new List<Vector3>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 6; y < height - 2; y++)
            {
                if (maze[x, y] == 0)
                {
                    openPositions.Add(new Vector2Int(x, y));
                }
            }
        }

        for (int i = 0; i < count; i++)
        {
            if (openPositions.Count == 0) break;

            Vector2Int pos = GetValidSpawnPosition(openPositions, spawnedEnemies, minDistance);
            if (pos == Vector2Int.zero) break; // No valid position found

            Vector3 spawnPos = new Vector3(pos.x - width / 2f + 0.5f, 0.85f, pos.y - height / 2f + 1);
            spawnedEnemies.Add(spawnPos);
            Instantiate(enemy, spawnPos, Quaternion.identity, transform);
        }
    }

    Vector2Int GetValidSpawnPosition(List<Vector2Int> openPositions, List<Vector3> spawnedEnemies, float minDistance)
    {
        for (int attempts = 0; attempts < openPositions.Count; attempts++)
        {
            int index = Random.Range(0, openPositions.Count);
            Vector2Int pos = openPositions[index];
            Vector3 spawnPos = new Vector3(pos.x - width / 2f + 0.5f, 0.85f, pos.y - height / 2f + 1);

            bool valid = true;
            foreach (Vector3 enemyPos in spawnedEnemies)
            {
                if (Vector3.Distance(spawnPos, enemyPos) < minDistance)
                {
                    valid = false;
                    break;
                }
            }

            if (valid)
            {
                openPositions.RemoveAt(index);
                return pos;
            }
        }

        return Vector2Int.zero; // No valid position found
    }
}