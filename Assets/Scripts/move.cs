using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed = 10f;

    private PlayerController playerController;

    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        if (!playerController.isGameOver)
        {
            transform.Translate(Vector3.back * Time.deltaTime * speed);
        }
    }
}