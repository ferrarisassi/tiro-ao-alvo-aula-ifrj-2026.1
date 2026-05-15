using UnityEngine;

public class Bullet : MonoBehaviour
{
    void Start()
    {

    }
    
    void OnCollisionEnter(Collision collision)
    {
        // Destroy bullet on any collision
        Destroy(gameObject);
    }
}