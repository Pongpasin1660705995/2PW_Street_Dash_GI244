using UnityEngine;

public class ScoreItemMover : MonoBehaviour
{
    public float speed = 10f;

    void Update()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime);

        if (transform.position.z < -10f)
        {
            ObstacleObjectPool.GetInstance().Release(gameObject);
        }
    }
}
