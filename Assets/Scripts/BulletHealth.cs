using UnityEngine;

public class BulletHealth : MonoBehaviour
{
    public int health = 1;

    public float invulnPeriod = 0;
    float invulnTime = 0;
    int correctLayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        correctLayer = gameObject.layer;
    }

    private void OnTriggerEnter2D()
    {
        Debug.Log("trigger");

        health--;
        invulnTime = invulnPeriod;
        gameObject.layer = 10;
    }

    // Update is called once per frame
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
            // Add points to score manager
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddEnemyKillPoints();
            }

            // Destroy the enemy game object
            Destroy(gameObject);
        }
    }
}
