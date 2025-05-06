using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    // Singleton instance
    public static ScoreManager Instance { get; private set; }

    // Score values
    private int currentScore = 0;
    private float gameTime = 0f; // Time in seconds

    // Points per kill based on level
    private readonly int[] basePointsPerLevel = { 10, 15, 20 }; // Level 1, 2, 3

    // Current score multiplier
    private float scoreMultiplier = 1.0f;

    // Time interval for multiplier increase (5 minutes in seconds)
    private readonly float multiplierInterval = 300f;
    private float nextMultiplierIncrease = 300f; // 5 minutes

    // Reference to current level
    private EnemyBehaviorType currentLevel;

    // Flag to check if we're in the game scene
    private bool isInGameScene = false;

    private void Awake()
    {
        // Simple singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Subscribe to scene change events
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        // Clean up event subscription
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Called when a scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if we're in the game scene
        isInGameScene = (scene.name == "Game");

        if (isInGameScene)
        {
            // Update the current level whenever we enter the game scene
            UpdateCurrentLevel();
        }
    }

    void Start()
    {
        UpdateCurrentLevel();
        ResetScore();
    }

    // Get the current level from LevelManager
    private void UpdateCurrentLevel()
    {
        // Get the current level behavior from LevelManager
        if (LevelManager.Instance != null)
        {
            currentLevel = LevelManager.Instance.GetCurrentBehavior();
            Debug.Log("ScoreManager updated level to: " + currentLevel);
        }
        else
        {
            Debug.LogWarning("LevelManager not found! Defaulting to Level 1.");
            currentLevel = EnemyBehaviorType.PathFinding;
        }
    }

    void Update()
    {
        // Only update timer when in the game scene
        if (isInGameScene)
        {
            // Update game time
            gameTime += Time.deltaTime;

            // Check if it's time to increase the multiplier
            if (gameTime >= nextMultiplierIncrease)
            {
                scoreMultiplier *= 1.5f;
                nextMultiplierIncrease += multiplierInterval;
                Debug.Log($"Score multiplier increased to: {scoreMultiplier}x");
            }
        }
    }

    // Add points for killing an enemy based on current level and multiplier
    public void AddEnemyKillPoints()
    {
        int levelIndex = (int)currentLevel;
        int pointsToAdd = Mathf.RoundToInt(basePointsPerLevel[levelIndex] * scoreMultiplier);
        currentScore += pointsToAdd;

        Debug.Log($"Enemy killed! +{pointsToAdd} points (Base: {basePointsPerLevel[levelIndex]}, Multiplier: {scoreMultiplier}x)");
    }

    // Get current score
    public int GetScore()
    {
        return currentScore;
    }

    // Get formatted time string (MM:SS)
    public string GetFormattedTime()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(gameTime);
        return string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
    }

    // Get formatted time string (HH:MM:SS)
    public string GetFormattedLongTime()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(gameTime);
        return string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
    }

    // Reset score and timer
    public void ResetScore()
    {
        currentScore = 0;
        gameTime = 0f;
        scoreMultiplier = 1.0f;
        nextMultiplierIncrease = multiplierInterval;
        Debug.Log("Score and timer reset");
    }

    // Display score and time on screen only when in the game scene
    private void OnGUI()
    {
        // Only show UI when in the game scene
        if (isInGameScene)
        {
            // Show score in top-right corner
            GUI.Label(new Rect(Screen.width - 200, 0, 200, 50), $"Skoor: {currentScore}");

            // Show time in top-right corner below score
            GUI.Label(new Rect(Screen.width - 200, 20, 200, 50), $"Aeg: {GetFormattedTime()}");

            // Show current multiplier
            GUI.Label(new Rect(Screen.width - 200, 40, 200, 50), $"Skoori korrutaja: {scoreMultiplier:F1}x");
        }
    }
}