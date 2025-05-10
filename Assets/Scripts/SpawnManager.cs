using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    public GameObject[] scorePrefabs;

    public Vector3 obstacleSpawnPos = new Vector3(0, 0, 30);
    public Vector3 scoreSpawnPos = new Vector3(0, 1, 30);
    public float xRange = 10f;

    public float startDelay = 5;
    public float repeatRate = 1;

    [Range(0f, 1f)]
    public float scoreSpawnChance = 0.6f;

    private ObstacleObjectPool obstacleObjectPool;

    void Start()
    {
        obstacleObjectPool = ObstacleObjectPool.GetInstance();

        // Preload obstacles
        foreach (var prefab in obstaclePrefabs)
        {
            for (int i = 0; i < 5; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.name = prefab.name;
                obstacleObjectPool.AddToPool(prefab.name, obj);
            }
        }

        // Preload score items
        foreach (var prefab in scorePrefabs)
        {
            for (int i = 0; i < 10; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.name = prefab.name;
                obstacleObjectPool.AddToPool(prefab.name, obj);
            }
        }

        InvokeRepeating(nameof(SpawnObject), startDelay, repeatRate);
    }

    void SpawnObject()
    {
        var player = GameObject.Find("Player")?.GetComponent<PlayerController>();
        if (player == null || player.isGameOver)
        {
            CancelInvoke(nameof(SpawnObject));
            return;
        }

        float roll = Random.value;
        GameObject selectedPrefab;
        string prefabName;
        Vector3 spawnPosition;

        if (roll < scoreSpawnChance && scorePrefabs.Length > 0)
        {
            int randomScoreIndex = Random.Range(0, scorePrefabs.Length);
            selectedPrefab = scorePrefabs[randomScoreIndex];
            prefabName = selectedPrefab.name;

            float randomX = Random.Range(-xRange, xRange);
            spawnPosition = new Vector3(randomX, scoreSpawnPos.y, scoreSpawnPos.z);
        }
        else
        {
            int randomObstacleIndex = Random.Range(0, obstaclePrefabs.Length);
            selectedPrefab = obstaclePrefabs[randomObstacleIndex];
            prefabName = selectedPrefab.name;

            spawnPosition = obstacleSpawnPos;
        }

        GameObject obj = obstacleObjectPool.Acquire(prefabName);

        if (obj != null)
        {
            obj.transform.position = spawnPosition;
            obj.transform.rotation = selectedPrefab.transform.rotation;

            var mover = obj.GetComponent<ObstacleMover>();
            if (mover != null)
                mover.speed = 40f;

            var scoreMover = obj.GetComponent<ScoreItemMover>();
            if (scoreMover != null)
                scoreMover.speed = 40f;
        }
    }
}