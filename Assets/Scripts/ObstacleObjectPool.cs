using System.Collections.Generic;
using UnityEngine;

public class ObstacleObjectPool : MonoBehaviour
{
    private static ObstacleObjectPool instance;

    private Dictionary<string, List<GameObject>> pooledObjects = new Dictionary<string, List<GameObject>>();

    public GameObject[] obstaclePrefabs;
    public int initialPoolSize = 5;

    public static ObstacleObjectPool GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<ObstacleObjectPool>();
        }
        return instance;
    }

    void Start()
    {
        foreach (GameObject prefab in obstaclePrefabs)
        {
            List<GameObject> pool = new List<GameObject>();

            for (int i = 0; i < initialPoolSize; i++)
            {
                GameObject p = Instantiate(prefab);
                p.SetActive(false);
                pool.Add(p);
            }

            pooledObjects[prefab.name] = pool;
        }
    }

    public GameObject Acquire(string prefabName)
    {
        if (pooledObjects.ContainsKey(prefabName))
        {
            List<GameObject> pool = pooledObjects[prefabName];

            if (pool.Count > 0)
            {
                GameObject p = pool[0];
                pool.RemoveAt(0);
                p.SetActive(true);
                return p;
            }
            else
            {
                // หา prefab ต้นฉบับจากชื่อ
                GameObject original = System.Array.Find(obstaclePrefabs, p => p.name == prefabName);
                if (original != null)
                {
                    GameObject newObj = Instantiate(original);
                    newObj.SetActive(true);
                    return newObj;
                }
            }
        }

        return null;
    }

    public void Release(GameObject p)
    {
        p.SetActive(false);
        string key = p.name.Replace("(Clone)", "").Trim(); // กำจัด (Clone)
        if (!pooledObjects.ContainsKey(key))
        {
            pooledObjects[key] = new List<GameObject>();
        }
        pooledObjects[key].Add(p);
    }
}
