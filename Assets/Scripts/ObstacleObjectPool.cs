using System.Collections.Generic;
using UnityEngine;

public class ObstacleObjectPool : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    public GameObject[] scorePrefabs;

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

    // เรียกเมื่อเริ่ม เพื่อเก็บ Prefab เข้า Pool
    public void RegisterPrefab(string key, GameObject prefab)
    {
        if (!prefabLookup.ContainsKey(key))
        {
            prefabLookup[key] = prefab;
        }
    }

    // เพิ่ม Object ไปยัง Pool
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

    // ขอ Object จาก Pool
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

        Debug.LogWarning("No available object or prefab registered for key: " + key);
        return null;
    }

    // คืน Object กลับ Pool
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
        else
        {
            Debug.LogWarning("Trying to release untracked object to pool.");
        }
    }

    // สำหรับลงทะเบียนทั้งหมดใน Start
    private void Start()
    {
        foreach (var prefab in obstaclePrefabs)
        {
            RegisterPrefab(prefab.name, prefab);
        }

        foreach (var prefab in scorePrefabs)
        {
            RegisterPrefab(prefab.name, prefab);
        }
    }
}
