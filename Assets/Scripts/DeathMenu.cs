using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathMenu : MonoBehaviour
{
    public static DeathMenu Instance { get; private set; }

    // Reference to the panel containing death menu elements
    public GameObject deathMenuPanel;

    // References to UI elements
    public Text finalScoreText;
    public Text timePlayedText;

    // References to buttons
    public Button restartButton;
    public Button levelSelectButton;

    private void Awake()
    {
        // Simple singleton pattern
        if (Instance == null)
        {
            Instance = this;
            // Not using DontDestroyOnLoad since we want a fresh death menu for each level
        }
        else
        {
            Destroy(gameObject);
        }

        // Hide the death menu at start
        if (deathMenuPanel != null)
        {
            deathMenuPanel.SetActive(false);
        }
    }

    private void Start()
    {
        // Set up button listeners
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartLevel);

        if (levelSelectButton != null)
            levelSelectButton.onClick.AddListener(ReturnToLevelSelect);
    }

    // Call this method when the player dies with no lives left
    public void ShowDeathMenu()
    {
        // Get final score and time from ScoreManager
        if (ScoreManager.Instance != null)
        {
            if (finalScoreText != null)
                finalScoreText.text = "Lõplik skoor: " + ScoreManager.Instance.GetScore();

            if (timePlayedText != null)
                timePlayedText.text = "Aeg: " + ScoreManager.Instance.GetFormattedTime();
        }

        // Show the death menu
        if (deathMenuPanel != null)
        {
            deathMenuPanel.SetActive(true);
        }

        // Pause the game
        Time.timeScale = 0f;
    }

    // Restart the current level
    public void RestartLevel()
    {
        // Unpause the game
        Time.timeScale = 1f;

        // Reset the score
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ResetScore();
        }

        // Reload the current scene
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    // Return to the level select screen
    public void ReturnToLevelSelect()
    {
        // Unpause the game
        Time.timeScale = 1f;

        // Load the level select scene
        SceneManager.LoadScene("LevelSelect");
    }
}