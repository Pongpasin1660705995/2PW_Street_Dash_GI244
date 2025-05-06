using UnityEngine;
using UnityEngine.SceneManagement;

public class MovingCredit : MonoBehaviour
{
    public RectTransform creditText; // อ้างอิงไปยัง UI Text หรือ TMP Text ที่ใช้แสดงเครดิต
    public float scrollSpeed = 50f; // ความเร็วในการเลื่อนเครดิต
    public float stopTime = 5f; // เวลาที่เครดิตจะเลื่อนก่อนหยุด
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

