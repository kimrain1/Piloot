using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public enum EnemyBehaviorType
{
    PathFinding,  // Level 1 - A* pathfinding
    Boid,         // Level 2 - BOID algorithm
    Patrol        // Level 3 - Patrol and global alert
}

public class EnemyController : MonoBehaviour
{
    // Common variables
    private Transform player;
    private NavMeshAgent agent;
    public float rotSpeed = 90f;

    // Current behavior type based on level selection
    private EnemyBehaviorType currentBehavior;

    // Patrol behavior variables
    public float detectionRadius = 10f;
    public float lostPlayerTime = 10f;
    private bool isPlayerInRange = false;
    private float timer = 0f;
    private Vector3 patrolPointA;
    private Vector3 patrolPointB;
    private bool movingToA = true;
    private static bool playerDetected = false;
    private float patrolTime = 3f;
    private float patrolTimer = 0f;

    // Boid behavior variables
    private List<Transform> nearbyEnemies = new List<Transform>();
    private int attackThreshold = 3;
    private bool isGathering = true;

    private void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        // Initialize patrol points if using patrol behavior
        patrolPointA = transform.position + new Vector3(10, 0, 0);
        patrolPointB = transform.position - new Vector3(10, 0, 0);

        // Get behavior type from LevelManager
        if (LevelManager.Instance != null)
        {
            currentBehavior = LevelManager.Instance.GetCurrentBehavior();
        }
        else
        {
            Debug.LogWarning("LevelManager not found! Defaulting to PathFinding behavior.");
            currentBehavior = EnemyBehaviorType.PathFinding;
        }
    }

    public void SetBehaviorType(EnemyBehaviorType behaviorType)
    {
        currentBehavior = behaviorType;
    }

    private void Update()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player")?.transform;
            if (player == null) return;
        }

        // Execute the appropriate behavior based on the current level
        switch (currentBehavior)
        {
            case EnemyBehaviorType.PathFinding:
                ExecutePathfindingBehavior();
                break;

            case EnemyBehaviorType.Boid:
                ExecuteBoidBehavior();
                break;

            case EnemyBehaviorType.Patrol:
                ExecutePatrolBehavior();
                break;
        }
    }

    #region Pathfinding Behavior (Level 1)

    private void ExecutePathfindingBehavior()
    {
        Vector3 targetPosition = new Vector3(player.position.x, player.position.y, transform.position.z);
        agent.SetDestination(targetPosition);
        RotateTowardsPlayer();
    }

    #endregion

    #region Boid Behavior (Level 2)

    private void ExecuteBoidBehavior()
    {
        // Find nearby enemies
        FindNearbyEnemies();

        // If there are enough enemies close to each other, trigger attack behavior
        if (nearbyEnemies.Count >= attackThreshold)
        {
            // Start attack behavior if enough enemies are grouped
            if (isGathering)
            {
                isGathering = false;
                MoveToAttackPosition();
            }
            BoidAttackPlayer();
        }
        else
        {
            // If not enough enemies, gather to form a group
            if (isGathering)
            {
                GatherWithEnemies();
            }
        }

        RotateTowardsPlayer();
    }

    void FindNearbyEnemies()
    {
        nearbyEnemies.Clear();
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in allEnemies)
        {
            if (enemy.transform != this.transform && Vector3.Distance(enemy.transform.position, transform.position) <= detectionRadius)
            {
                nearbyEnemies.Add(enemy.transform);
            }
        }
    }

    void BoidAttackPlayer()
    {
        // Make sure the enemy is moving towards the player
        if (player != null)
        {
            agent.SetDestination(player.position);
        }
    }

    void GatherWithEnemies()
    {
        if (nearbyEnemies.Count == 0)
            return; // Exit early if no nearby enemies

        // Calculate the group center based on nearby enemies' positions
        Vector3 groupCenter = Vector3.zero;
        foreach (Transform enemy in nearbyEnemies)
        {
            groupCenter += enemy.position;
        }
        groupCenter /= nearbyEnemies.Count;

        // Check if the group center is too far from the NavMesh or outside valid bounds
        if (Vector3.Distance(groupCenter, transform.position) < 50f) // Example threshold
        {
            // Check if the group center is on the NavMesh
            NavMeshHit hit;
            if (NavMesh.SamplePosition(groupCenter, out hit, 10f, NavMesh.AllAreas))
            {
                // Set the destination to the valid position on the NavMesh
                agent.SetDestination(hit.position);
            }
            else
            {
                // If the group center is invalid, fallback to a default position
                agent.SetDestination(transform.position);
            }
        }
        else
        {
            // If group center is too far, fallback to a default position
            agent.SetDestination(transform.position);
        }
    }

    void MoveToAttackPosition()
    {
        // Move towards a group position or a designated attack position
        Vector3 groupPosition = Vector3.zero;
        foreach (Transform enemy in nearbyEnemies)
        {
            groupPosition += enemy.position;
        }

        groupPosition /= nearbyEnemies.Count;
        agent.SetDestination(groupPosition);
    }

    #endregion

    #region Patrol Behavior (Level 3)

    private void ExecutePatrolBehavior()
    {
        patrolTimer += Time.deltaTime;

        // Check if the player is within detection radius
        if (Vector3.Distance(transform.position, player.position) <= detectionRadius)
        {
            isPlayerInRange = true;
            timer = 0f; // Reset timer as player is detected
            playerDetected = true; // Mark that player has been detected by any enemy
        }
        else
        {
            if (isPlayerInRange)
            {
                timer += Time.deltaTime;
                if (timer >= lostPlayerTime)
                {
                    isPlayerInRange = false;
                    playerDetected = false; // Reset detection state for all enemies
                    PatrolMovement(); // Start patrolling after losing the player
                }
            }
        }

        // If player is in range, all enemies attack the player
        if (playerDetected)
        {
            AttackPlayer();
        }
        else
        {
            PatrolMovement();
        }

        // Face the player while moving towards or attacking
        RotateTowardsPlayer();
    }

    void PatrolMovement()
    {
        // Move to the current patrol point
        if (patrolTimer >= patrolTime)
        {
            if (movingToA)
            {
                agent.SetDestination(patrolPointA);
                if (Vector3.Distance(transform.position, patrolPointA) < 1f)
                {
                    movingToA = false; // Switch direction when point A is reached
                    patrolTimer = 0f; // Reset patrol timer
                }
            }
            else
            {
                agent.SetDestination(patrolPointB);
                if (Vector3.Distance(transform.position, patrolPointB) < 1f)
                {
                    movingToA = true; // Switch direction when point B is reached
                    patrolTimer = 0f; // Reset patrol timer
                }
            }
        }
    }

    void AttackPlayer()
    {
        // Attack behavior
        agent.SetDestination(player.position); // Move towards the player
    }

    #endregion

    // Common rotation function used by all behaviors
    void RotateTowardsPlayer()
    {
        Vector3 dir = player.position - transform.position;
        dir.Normalize();

        float zAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        Quaternion desiredRot = Quaternion.Euler(0, 0, zAngle);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRot, rotSpeed * Time.deltaTime);
    }
}