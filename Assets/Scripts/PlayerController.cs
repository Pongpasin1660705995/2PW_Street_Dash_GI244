// Lecture Note
// [1] New Input System in Unity: https://learn.unity.com/tutorial/getting-started-with-the-new-input-system

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // hello classroom
        Debug.Log("Hello GI244");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.forward);
    }
}
