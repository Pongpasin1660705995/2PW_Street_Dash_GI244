using System.Collections.Generic;
using UnityEngine;

public class ObstacleObjectPool : MonoBehaviour
{
    private static ObstacleObjectPool instance;
    public static ObstacleObjectPool GetInstance()
    {
        if (instance == null)
        {
            GameObject obj = new GameObject("ObstacleObjectPool");
            instance = obj.AddComponent<ObstacleObjectPool>();
        }
        return instance;
    }

    private Dictionary<string, Queue<GameObject>> pool = new Dictionary<string, Queue<GameObject>>();

    // ฟังก์ชันนี้ใช้สำหรับการเพิ่ม Object ไปที่ Pool
    public void AddToPool(string key, GameObject obj)
    {
        obj.SetActive(false);

        if (!pool.ContainsKey(key))
        {
            pool[key] = new Queue<GameObject>();
        }

        pool[key].Enqueue(obj);
    }

    // ฟังก์ชันนี้ใช้ดึง Object จาก Pool
    public GameObject Acquire(string key)
    {
        if (pool.ContainsKey(key) && pool[key].Count > 0)
        {
            GameObject obj = pool[key].Dequeue();
            obj.SetActive(true);
            return obj;
        }

        Debug.LogWarning("No available object in pool for key: " + key);
        return null;
    }

    // ฟังก์ชันนี้ใช้สำหรับคืน Object ไปที่ Pool
    public void Release(GameObject obj)
    {
        obj.SetActive(false);
        string key = obj.name;

        if (!pool.ContainsKey(key))
        {
            pool[key] = new Queue<GameObject>();
        }

        pool[key].Enqueue(obj);
    }
}
