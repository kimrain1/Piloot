using UnityEngine;

public class playerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    GameObject playerInstance;

    public int numLives = 4;

    float respawnTimer;
    bool gameOver = false;

    void Start()
    {
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        numLives--;
        respawnTimer = 1;
        playerInstance = (GameObject)Instantiate(playerPrefab, transform.position, Quaternion.identity);
    }

    void Update()
    {
        if (playerInstance == null && numLives > 0)
        {
            respawnTimer -= Time.deltaTime;

            if (respawnTimer <= 0)
            {
                SpawnPlayer();
            }
        }
        else if (playerInstance == null && numLives <= 0 && !gameOver)
        {
            gameOver = true;

            DeathMenu deathMenu = FindObjectOfType<DeathMenu>();
            if (deathMenu != null)
            {
                deathMenu.ShowDeathMenu();
            }
            else
            {
                Debug.LogWarning("DeathMenu not found in scene!");
            }
        }
    }

    private void OnGUI()
    {
        if (playerInstance != null || numLives > 0)
        {
            GUI.Label(new Rect(0, 0, 100, 50), "Elusid alles: " + numLives);
        }
    }
}