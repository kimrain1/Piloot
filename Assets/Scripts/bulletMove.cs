using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class bulletMove : MonoBehaviour
{
    public float maxSpeed = 5f;

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;

        Vector3 velocity = new Vector3(0, maxSpeed * Time.deltaTime, 0);

        pos += transform.rotation * velocity;

        transform.position = pos;
    }
}
