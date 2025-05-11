using System.Collections.Generic;
using UnityEngine;

public class ObstacleObjectPool : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;

    private static ObstacleObjectPool instance;

    public static ObstacleObjectPool GetInstance()
    {
        if (instance == null)
        {
            instance = FindFirstObjectByType<ObstacleObjectPool>();
            if (instance == null)
            {
                GameObject obj = new GameObject("ObstacleObjectPool");
                instance = obj.AddComponent<ObstacleObjectPool>();
            }
        }
        return instance;
    }

    private Dictionary<string, Queue<GameObject>> pool = new Dictionary<string, Queue<GameObject>>();
    private Dictionary<string, GameObject> prefabLookup = new Dictionary<string, GameObject>();
    private Dictionary<GameObject, string> reverseLookup = new Dictionary<GameObject, string>();

    public void RegisterPrefab(string key, GameObject prefab)
    {
        if (!prefabLookup.ContainsKey(key))
        {
            prefabLookup[key] = prefab;
        }
    }

    public void AddToPool(string key, GameObject obj)
    {
        obj.SetActive(false);

        if (!pool.ContainsKey(key))
        {
            pool[key] = new Queue<GameObject>();
        }

        pool[key].Enqueue(obj);
        reverseLookup[obj] = key;
    }

    public GameObject Acquire(string key)
    {
        if (pool.ContainsKey(key) && pool[key].Count > 0)
        {
            GameObject obj = pool[key].Dequeue();
            obj.SetActive(true);
            return obj;
        }

        if (prefabLookup.ContainsKey(key))
        {
            GameObject newObj = Instantiate(prefabLookup[key]);
            newObj.name = key;
            reverseLookup[newObj] = key;
            return newObj;
        }

        return null;
    }

    public void Release(GameObject obj)
    {
        obj.SetActive(false);

        if (reverseLookup.TryGetValue(obj, out string key))
        {
            if (!pool.ContainsKey(key))
            {
                pool[key] = new Queue<GameObject>();
            }

            pool[key].Enqueue(obj);
        }
    }

    private void Start()
    {
        foreach (var prefab in obstaclePrefabs)
        {
            RegisterPrefab(prefab.name, prefab);
        }
    }
}
