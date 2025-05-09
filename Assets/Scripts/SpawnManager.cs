using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    public Vector3 spawnPos;
    public float startDelay = 5;
    public float repeatRate = 1;

    private ObstacleObjectPool obstacleObjectPool;

    void Start()
    {
        obstacleObjectPool = ObstacleObjectPool.GetInstance();
        InvokeRepeating(nameof(SpawnObstacle), startDelay, repeatRate);
    }

    void SpawnObstacle()
    {
        //if (FindObjectOfType<PlayerController>().gameOver)
        //{
        //    CancelInvoke(nameof(SpawnObstacle));
        //    return;
        //}

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CancelInvoke(nameof(SpawnObstacle));
            return;
        }

        int randomIndex = Random.Range(0, obstaclePrefabs.Length);
        GameObject randomPrefab = obstaclePrefabs[randomIndex];
        GameObject obstacle = obstacleObjectPool.Acquire(randomPrefab.name);


        if (obstacle != null)
        {
            obstacle.transform.position = spawnPos;
            obstacle.transform.rotation = randomPrefab.transform.rotation;

            var mover = obstacle.GetComponent<ObstacleMover>();
            if (mover != null)
            {
                mover.speed = 10f;
            }
        }
    }
}