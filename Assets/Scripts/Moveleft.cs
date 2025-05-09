using UnityEngine;

public class Moveleft : MonoBehaviour
{
    public float speed = 5f;
    private float destroyDistance = 60f;
    private float startX;

    void Start()
    {
        startX = transform.position.x;
    }

    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        // �������͹价ҧ�����Թ 60 ˹��� ������µ���ͧ
        if (Mathf.Abs(transform.position.x - startX) >= destroyDistance)
        {
            Destroy(gameObject);
        }
    }
}
