using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float xRange = 10f;

    public GameObject projectilePrefab;

    // Game system variables
    public int lives = 3;
    private int score = 0;
    private float distance = 0f;
    private float highDistance = 0f;
    public bool isGameOver = false;

    public TextMeshProUGUI ScoreText;
    public GameObject GameOverUI;
    public TextMeshProUGUI GameOverScoreText;
    public TextMeshProUGUI GameOverHighScoreText;
    public TextMeshProUGUI DistanceText;
    public TextMeshProUGUI GameOverDistanceText;
    public TextMeshProUGUI GameOverHighDistanceText;
    public Button RestartButton;
    public Button MainMenuButton;

    // UI สำหรับแสดงหัวใจ
    public GameObject heartPrefab;
    public Transform heartContainer;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    private List<Image> heartImages = new List<Image>();

    private float horizontalInput;
    private InputAction moveAction;

    private void Awake()
    {
        // ใช้ Input System ใหม่
        moveAction = InputSystem.actions.FindAction("Move");
    }

    void Start()
    {
        UpdateUI();
        GameOverUI.SetActive(false);

        RestartButton.onClick.AddListener(RestartGame);
        MainMenuButton.onClick.AddListener(GoToMainMenu);

        GenerateHearts();
        UpdateHearts();
    }

    void Update()
    {
        if (isGameOver) return;

        horizontalInput = moveAction.ReadValue<Vector2>().x;

        transform.Translate(horizontalInput * speed * Time.deltaTime * Vector3.right);

        // จำกัดการเคลื่อนไหวในแกน x
        if (transform.position.x < -xRange)
            transform.position = new Vector3(-xRange, transform.position.y, transform.position.z);
        if (transform.position.x > xRange)
            transform.position = new Vector3(xRange, transform.position.y, transform.position.z);

        // เพิ่มระยะทาง
        distance += Time.deltaTime;

        UpdateUI();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isGameOver) return;

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            lives--;
            ObstacleObjectPool.GetInstance().Release(collision.gameObject);

            UpdateHearts();

            if (lives <= 0)
            {
                GameOver();
            }
        }

        if (collision.gameObject.CompareTag("Score"))
        {
            score++;

            // ปล่อยกลับเข้า pool แทนการ Destroy
            ObstacleObjectPool.GetInstance().Release(collision.gameObject);
        }

    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        GameOverUI.SetActive(true);

        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
        }

        // ระยะทางสูงสุด
        float savedHighDistance = PlayerPrefs.GetFloat("HighDistance", 0f);
        if (distance > savedHighDistance)
        {
            savedHighDistance = distance;
            PlayerPrefs.SetFloat("HighDistance", savedHighDistance);
        }

        GameOverScoreText.text = "Score: " + score;
        GameOverHighScoreText.text = "High Score: " + highScore;

        if (GameOverDistanceText != null)
            GameOverDistanceText.text = "Distance: " + distance.ToString("F1") + " m";
        if (GameOverHighDistanceText != null)
            GameOverHighDistanceText.text = "High Distance: " + savedHighDistance.ToString("F1") + " m";

        Time.timeScale = 0f;
    }

    void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    void UpdateUI()
    {
        if (ScoreText != null)
            ScoreText.text = "Score: " + score;

        if (DistanceText != null)
            DistanceText.text = "Distance: " + distance.ToString("F1") + " m";
    }
    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }
    void GenerateHearts()
    {
        foreach (Transform child in heartContainer)
        {
            Destroy(child.gameObject);
        }
        heartImages.Clear();

        for (int i = 0; i < lives; i++)
        {
            GameObject heart = Instantiate(heartPrefab, heartContainer);
            Image img = heart.GetComponent<Image>();
            heartImages.Add(img);
        }
    }

    void UpdateHearts()
    {
        for (int i = 0; i < heartImages.Count; i++)
        {
            if (i < lives)
                heartImages[i].sprite = fullHeart;
            else
                heartImages[i].sprite = emptyHeart;
        }
    }
}