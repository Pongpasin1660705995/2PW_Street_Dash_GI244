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
    public TextMeshProUGUI HistoryText;
    public TextMeshProUGUI NewRecordText;

    public Button RestartButton;
    public Button MainMenuButton;

    public GameObject heartPrefab;

    public Transform heartContainer;

    public Sprite fullHeart;
    public Sprite emptyHeart;

    public AudioSource audioSource;
    public AudioClip scoreClip;
    public AudioClip hitClip;
    public AudioClip gameOverClip;


    private List<Image> heartImages = new List<Image>();

    private float horizontalInput;
    private InputAction moveAction;

    private void Awake()
    {
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

        if (transform.position.x < -xRange)
            transform.position = new Vector3(-xRange, transform.position.y, transform.position.z);
        if (transform.position.x > xRange)
            transform.position = new Vector3(xRange, transform.position.y, transform.position.z);

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

            if (hitClip != null && audioSource != null)
                audioSource.PlayOneShot(hitClip);

            UpdateHearts();

            if (lives <= 0)
            {
                GameOver();
            }
        }

        if (collision.gameObject.CompareTag("Score"))
        {
            score++;

            if (scoreClip != null && audioSource != null)
                audioSource.PlayOneShot(scoreClip);

            Destroy(collision.gameObject);
            UpdateUI();
        }
    }


    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        GameOverUI.SetActive(true);

        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        bool isNewHighScore = false;
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            isNewHighScore = true;
        }

        float savedHighDistance = PlayerPrefs.GetFloat("HighDistance", 0f);
        bool isNewHighDistance = false;
        if (distance > savedHighDistance)
        {
            savedHighDistance = distance;
            PlayerPrefs.SetFloat("HighDistance", savedHighDistance);
            isNewHighDistance = true;
        }

        GameOverScoreText.text = "Score: " + score;
        GameOverHighScoreText.text = "High Score: " + highScore;

        if (GameOverDistanceText != null)
            GameOverDistanceText.text = "Distance: " + distance.ToString("F1") + " m";
        if (GameOverHighDistanceText != null)
            GameOverHighDistanceText.text = "High Distance: " + savedHighDistance.ToString("F1") + " m";

        
        if (NewRecordText != null)
        {
            if (isNewHighScore && isNewHighDistance)
                NewRecordText.text = "🎉 New High Score & Longest Distance!";
            else if (isNewHighScore)
                NewRecordText.text = "🎉 New High Score!";
            else if (isNewHighDistance)
                NewRecordText.text = "🎉 New Longest Distance!";
            else
                NewRecordText.text = "Keep trying to beat your record!";
        }

        if (gameOverClip != null && audioSource != null)
            audioSource.PlayOneShot(gameOverClip);

        Time.timeScale = 0f;
        SaveScoreToHistory(score);
        DisplayScoreHistory();
    }

    void SaveScoreToHistory(int score)
    {
        string history = PlayerPrefs.GetString("ScoreHistory", "");
        string newEntry = $"{score}:{distance:F1}";
        
        List<string> entries = new List<string>(history.Split(','));
        if (string.IsNullOrEmpty(history))
            entries.Clear();

        entries.Add(newEntry);

        if (entries.Count > 5)
            entries.RemoveAt(0);

        string updatedHistory = string.Join(",", entries);
        PlayerPrefs.SetString("ScoreHistory", updatedHistory);
    }

    void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }
    void DisplayScoreHistory()
    {
        string history = PlayerPrefs.GetString("ScoreHistory", "");
        if (string.IsNullOrEmpty(history))
        {
            HistoryText.text = "History:\nNo previous games.";
            return;
        }

        string[] entries = history.Split(',');
        HistoryText.text = "History:\n";

        for (int i = 0; i < entries.Length; i++)
        {
            string[] parts = entries[i].Split(':');
            if (parts.Length == 2)
            {
                string score = parts[0];
                string dist = parts[1];
                HistoryText.text += $"Round {i + 1} - Score: {score}, Distance: {dist} m\n";
            }
        }
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