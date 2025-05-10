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

    // �ѧ��ѹ���������Ѻ������� Object 价�� Pool
    public void AddToPool(string key, GameObject obj)
    {
        obj.SetActive(false);

        if (!pool.ContainsKey(key))
        {
            pool[key] = new Queue<GameObject>();
        }

        pool[key].Enqueue(obj);
    }

    // �ѧ��ѹ�����֧ Object �ҡ Pool
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

    // �ѧ��ѹ���������Ѻ�׹ Object 价�� Pool
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
