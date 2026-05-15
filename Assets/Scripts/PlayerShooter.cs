using UnityEngine;
using UnityEngine.UI;

public class PlayerShooter : MonoBehaviour
{
    [Header("Shooting Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 50f;
    public float fireRate = 0.2f;
    public int maxAmmo = 30;
    public float reloadTime = 1.5f;
    
    [Header("UI Elements")]
    public Text ammoText;
    public Text scoreText;
    public Text gameOverText;
    public Button restartButton;
    
    [Header("Game Settings")]
    public int scorePerHit = 10;
    public float gameDuration = 60f;
    
    private int currentAmmo;
    private int currentScore;
    private float nextFireTime;
    private bool isReloading = false;
    private float gameTimer;
    private bool isGameActive = true;
    
    void Start()
    {
        currentAmmo = maxAmmo;
        currentScore = 0;
        gameTimer = gameDuration;
        
        UpdateUI();
        
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);
            
        if (gameOverText != null)
            gameOverText.gameObject.SetActive(false);
    }
    
    void Update()
    {
        if (!isGameActive) return;
        
        // Timer
        gameTimer -= Time.deltaTime;
        if (gameTimer <= 0)
        {
            EndGame();
            return;
        }
        
        // Shooting
        if (Input.GetButtonDown("Fire1") && !isReloading && currentAmmo > 0 && Time.time >= nextFireTime)
        {
            Shoot();
        }
        
        // Reload
        if (Input.GetKeyDown(KeyCode.R) && !isReloading && currentAmmo < maxAmmo)
        {
            StartCoroutine(Reload());
        }
        
        // Update UI timer
        if (ammoText != null)
            ammoText.text = $"Ammo: {currentAmmo}/{maxAmmo}\nTime: {Mathf.CeilToInt(gameTimer)}s";
    }
    
    void Shoot()
    {
        nextFireTime = Time.time + fireRate;
        currentAmmo--;
        
        // Create bullet
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        
        if (rb != null)
        {
            rb.velocity = firePoint.forward * bulletSpeed;
        }
        
        // Add muzzle flash effect (optional)
        // Play shooting sound (optional)
        
        Destroy(bullet, 5f); // Clean up bullets after 5 seconds
        
        UpdateUI();
    }
    
    System.Collections.IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
        UpdateUI();
    }
    
    public void AddScore(int points)
    {
        currentScore += points;
        UpdateUI();
    }
    
    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = $"Score: {currentScore}";
    }
    
    void EndGame()
    {
        isGameActive = false;
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true);
            gameOverText.text = $"Game Over!\nFinal Score: {currentScore}";
        }
        if (restartButton != null)
            restartButton.gameObject.SetActive(true);
    }
    
    void RestartGame()
    {
        currentScore = 0;
        currentAmmo = maxAmmo;
        gameTimer = gameDuration;
        isGameActive = true;
        isReloading = false;
        
        if (gameOverText != null)
            gameOverText.gameObject.SetActive(false);
        if (restartButton != null)
            restartButton.gameObject.SetActive(false);
            
        UpdateUI();
    }
}