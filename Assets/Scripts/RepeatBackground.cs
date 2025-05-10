using UnityEngine;

public class RepeatBackground : MonoBehaviour
{
    private Vector3 startPos;
    private float repeatLength;
    private PlayerController playerController;

    public float speed;

    void Start()
    {
        startPos = transform.position;
        repeatLength = GetComponent<BoxCollider>().size.z / 1.5f;

        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        if (playerController != null && !playerController.isGameOver)
        {
            transform.Translate(Vector3.back * speed * Time.deltaTime);

            if (transform.position.z < startPos.z - repeatLength)
            {
                transform.position = startPos;
            }
        }
    }
}
