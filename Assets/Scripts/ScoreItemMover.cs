using UnityEngine;

public class ScoreItemMover : MonoBehaviour
{
    public float speed = 10f;
    private float leftBound = -150f;

    void Update()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime);

        if (transform.position.z < leftBound)
        {
            ObstacleObjectPool.GetInstance().Release(gameObject);
        }
    }
}
