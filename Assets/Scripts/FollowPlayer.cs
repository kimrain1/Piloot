using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Transform player;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;
    }

    void LateUpdate()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player")?.transform;
            if (player == null) return;
        }

        Vector3 targetPosition = new Vector3(player.position.x, player.position.y, transform.position.z) + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
    }
}
