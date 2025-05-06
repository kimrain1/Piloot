using UnityEngine;

public class healthControl : MonoBehaviour
{
    public int health = 1;

    public float invulnPeriod = 0;
    float invulnTime = 0;
    int correctLayer;

    public GameObject explosionPrefab;

    void Start()
    {
        correctLayer = gameObject.layer;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("trigger");

        if (other.gameObject.CompareTag("Wall"))
        {
            return; // Do nothing when hitting a wall
        }

        health--;
        invulnTime = invulnPeriod;
        gameObject.layer = 10;
    }

    void Update()
    {
        invulnTime -= Time.deltaTime;
        if (invulnTime <= 0)
        {
            gameObject.layer = correctLayer;
        }

        if (health <= 0)
        {
            Die();
        }

        void Die()
        {
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddEnemyKillPoints();
            }
            if (explosionPrefab != null)
            {
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }

    }
}
