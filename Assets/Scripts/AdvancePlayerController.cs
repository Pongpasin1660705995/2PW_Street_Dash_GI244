// Lecture Note
// [1] New Input System in Unity: https://learn.unity.com/tutorial/getting-started-with-the-new-input-system

using UnityEngine;
using UnityEngine.InputSystem;

public class AdvancePlayerController : MonoBehaviour
{
    // [1] Define the car settings
    [Header("Car Settings")]
    public float maxSpeed = 20.0f;  
    public float acceleration = 10.0f;
    public float deceleration = 15.0f;
    public float turnSpeed = 180.0f;
    public float brakingForce = 20.0f;

    // [2] Define the current state of the car
    [Header("Current State")]
    private float currentSpeed = 0.0f;
    private float horizontalInput = 0;
    private float forwardInput = 0;
    private bool isBraking = false;

    void Update()
    {
        // [3] Get input values
        InputAction moveAction = InputSystem.actions.FindAction("Move");
        Vector2 input = moveAction.ReadValue<Vector2>();
        horizontalInput = input.x;
        forwardInput = input.y;

        // [4] Handle braking
        isBraking = Input.GetKey(KeyCode.Space);

        // [5] Apply acceleration/deceleration
        // and Calculate currentSpeed
        if (forwardInput != 0)
        {
            // [6] Apply acceleration to currentSpeed

        }
        else
        {
            // [7] Natural deceleration when no input

        }

        // [8] Apply braking
        if (isBraking)
        {

        }

        // [9] Clamp speed


        // [10] **Apply movement**


        // [11] Apply steering (only when moving)

    }
}
