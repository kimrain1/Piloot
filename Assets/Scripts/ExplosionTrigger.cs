using UnityEngine;

public class ExplosionTrigger : MonoBehaviour
{
    public float lifeTime = 0.3f;
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
