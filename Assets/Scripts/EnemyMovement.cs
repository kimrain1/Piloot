using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float rotSpeed = 90f;
    public float moveSpeed = 3f; // Speed for forward movement

    private Transform player;

    void Update()
    {
        // Find the player if not already assigned
        if (player == null)
        {
            GameObject go = GameObject.FindWithTag("Player");
            if (go != null)
            {
                player = go.transform;
            }
        }

        if (player == null) return;

        // Calculate direction towards player
        Vector3 dir = player.position - transform.position;
        dir.Normalize();

        // Calculate desired rotation angle
        float zAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        Quaternion desiredRot = Quaternion.Euler(0, 0, zAngle);

        // Rotate towards the player smoothly
        transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRot, rotSpeed * Time.deltaTime);

        // Move forward in the direction the object is facing
        transform.position += transform.up * moveSpeed * Time.deltaTime;
    }
}
