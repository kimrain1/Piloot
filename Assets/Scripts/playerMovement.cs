using UnityEngine;
using System.Collections;

public class playerMovement : MonoBehaviour
{
    public float maxSpeed = 5f;
    float rotSpeed = 180f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // ROTATE the ship. 
        // Grab our rotation quaternion
        Quaternion rot = transform.rotation; 
        // Grab the Z euler angle
        float z = rot.eulerAngles.z; 
        // Change the Z angle based on input
        z -= Input.GetAxis("Horizontal") * rotSpeed * Time.deltaTime; 
        // Recreate the quaternion
        rot = Quaternion. Euler( 0, 0, z ); 
        // Feed the quaternion into our rotation
        transform.rotation = rot; 
        // MOVE the ship.
        Vector3 pos = transform.position;

        Vector3 velocity = new Vector3(0, Input.GetAxis("Vertical") * maxSpeed * Time.deltaTime, 0);

        pos += rot * velocity;

        transform.position = pos;
    }
}
