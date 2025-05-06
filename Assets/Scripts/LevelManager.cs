using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    private EnemyBehaviorType currentBehavior;

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

    public void SelectLevel(int levelIndex)
    {
        currentBehavior = (EnemyBehaviorType)levelIndex;
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public EnemyBehaviorType GetCurrentBehavior()
    {
        return currentBehavior;
    }

    public void QuitGame()
    {
        Debug.Log("Exiting game...");
        Application.Quit();
    }
}