using UnityEngine;
using UnityEngine.SceneManagement;

public class MovingCredit : MonoBehaviour
{
    public RectTransform creditText;
    public float scrollSpeed = 50f;
    public float stopTime = 5f;
    public float countdownDuration = 5f; 
    
    private float elapsedTime = 0f;
    private bool isScrolling = true;

    void Start()
    {
        Invoke("StartCountdown", stopTime);
    }

    void StartCountdown()
    {
        Invoke("LoadNextScene", countdownDuration);
    }

    void Update()
    {
        if (isScrolling)
        {
            creditText.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            
            if (elapsedTime >= stopTime)
            {
                isScrolling = false;
            }
        }
    }
    
    void LoadNextScene()
    {
        SceneManager.LoadScene("Main Menu");
    }
}

