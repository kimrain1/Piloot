using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    // Reference to the panel containing level select buttons
    public GameObject levelSelectPanel;

    // References to buttons
    public Button level1Button;
    public Button level2Button;
    public Button level3Button;

    public static LevelSelect Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Ensure there's a LevelManager in the scene
    private void Start()
    {
        // Check if LevelManager exists, if not create one
        if (LevelManager.Instance == null)
        {
            GameObject levelManagerObj = new GameObject("LevelManager");
            levelManagerObj.AddComponent<LevelManager>();
        }

        // Set up button listeners
        if (level1Button != null)
            level1Button.onClick.AddListener(LoadLevel1);

        if (level2Button != null)
            level2Button.onClick.AddListener(LoadLevel2);

        if (level3Button != null)
            level3Button.onClick.AddListener(LoadLevel3);

        // Add scene loaded event listener
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Initial check of current scene
        CheckCurrentScene();
    }

    private void OnDestroy()
    {
        // Remove event listener when object is destroyed
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Called when a new scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckCurrentScene();
    }

    private void CheckCurrentScene()
    {
        if (levelSelectPanel != null)
        {
            bool isLevelSelectScene = SceneManager.GetActiveScene().name == "LevelSelect";
            levelSelectPanel.SetActive(isLevelSelectScene);
        }
    }

    public void LoadLevel1()
    {
        // Reset score before starting new level
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ResetScore();
        }

        // Set behavior type to PathFinding and load Game scene
        LevelManager.Instance.SelectLevel(0);
        SceneManager.LoadScene("Game");
    }

    public void LoadLevel2()
    {
        // Reset score before starting new level
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ResetScore();
        }

        // Set behavior type to Boid and load Game scene
        LevelManager.Instance.SelectLevel(1);
        SceneManager.LoadScene("Game");
    }

    public void LoadLevel3()
    {
        // Reset score before starting new level
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ResetScore();
        }

        // Set behavior type to Patrol and load Game scene
        LevelManager.Instance.SelectLevel(2);
        SceneManager.LoadScene("Game");
    }
}