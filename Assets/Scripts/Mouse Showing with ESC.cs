using UnityEngine;

public class MouseShowingwithESC : MonoBehaviour
{
    private bool isCursorVisible = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isCursorVisible = !isCursorVisible;
            ToggleCursor(isCursorVisible);
        }
    }

    void ToggleCursor(bool visible)
    {
        Cursor.visible = visible;
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
