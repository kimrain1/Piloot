using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        EnemyController[] enemies = FindObjectsOfType<EnemyController>();

        if (LevelManager.Instance != null)
        {
            EnemyBehaviorType behaviorType = LevelManager.Instance.GetCurrentBehavior();
            Debug.Log("Setting enemies to behavior: " + behaviorType);

            foreach (EnemyController enemy in enemies)
            {
                enemy.SetBehaviorType(behaviorType);
            }
        }
        else
        {
            Debug.LogError("LevelManager not found!");
        }
    }
}