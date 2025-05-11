using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    public float speed = 10f;
    private float leftBound = -150f;

    void Update()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime);

        if (transform.position.z < leftBound)
        {
            // คืนอุปสรรคกลับเข้า object pool
            ObstacleObjectPool.GetInstance().Release(gameObject);
        }
    }
}