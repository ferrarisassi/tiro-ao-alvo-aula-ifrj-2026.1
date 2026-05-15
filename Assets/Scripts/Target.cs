using UnityEngine;

public class Target : MonoBehaviour
{
    [Header("Target Settings")]
    public int health = 1;
    public int pointsValue = 10;
    public float moveSpeed = 3f;
    public float respawnTime = 2f;
    
    [Header("Movement Type")]
    public bool moveHorizontal = true;
    public bool moveVertical = false;
    public float moveRange = 5f;
    
    private Vector3 startPosition;
    private float direction = 1f;
    private FPSAimController playerShooter; // Changed from PlayerShooter to FPSAimController
    
    void Start()
    {
        startPosition = transform.position;
        playerShooter = FindObjectOfType<FPSAimController>(); // Changed to FPSAimController
    }
    
    void Update()
    {
        // Move target
        Vector3 newPosition = transform.position;
        
        if (moveHorizontal)
        {
            newPosition.x += direction * moveSpeed * Time.deltaTime;
            if (Mathf.Abs(newPosition.x - startPosition.x) >= moveRange)
                direction *= -1;
        }
        
        if (moveVertical)
        {
            newPosition.y += direction * moveSpeed * Time.deltaTime;
            if (Mathf.Abs(newPosition.y - startPosition.y) >= moveRange)
                direction *= -1;
        }
        
        transform.position = newPosition;
        
        // Optional: Rotate target
        transform.Rotate(Vector3.up, 180 * Time.deltaTime);
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            health--;
            
            if (health <= 0)
            {
                // Add score
                if (playerShooter != null)
                    playerShooter.AddScore(pointsValue);
                
                // Play hit effect
                Debug.Log("Target hit! +" + pointsValue + " points");
                
                // Destroy bullet
                Destroy(other.gameObject);
                
                // Respawn target
                StartCoroutine(RespawnTarget());
            }
            else
            {
                // Destroy bullet even if target not destroyed
                Destroy(other.gameObject);
            }
        }
    }
    
    System.Collections.IEnumerator RespawnTarget()
    {
        gameObject.SetActive(false);
        yield return new WaitForSeconds(respawnTime);
        
        health = 1; // Reset health
        transform.position = startPosition;
        gameObject.SetActive(true);
    }
}