using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthManager : MonoBehaviour
{
    public Slider healthSlider;
    public TextMeshProUGUI healthText;
    public PlayerHitEffect hitEffect;
    public Transform popupSpawnPoint; // im Inspector zuweisen



    public int maxHealth = 100;
    public int currentHealth;
    private UpgradeManager upgradeManager;
    public int currentShield = 0;

    public GameOverManager gameOverManager;
    public TextMeshProUGUI shieldText;


    [Header("Damage Popup")]
    public DamagePopupSpawner damagePopupSpawner;  

    void Start()
    {
        upgradeManager = FindFirstObjectByType<UpgradeManager>();
        int gluttonyLevel = upgradeManager.GetUpgradeLevel("Gluttony");
        maxHealth = maxHealth + (5 * gluttonyLevel);
        currentHealth = maxHealth;
        UpdateHealthUI();
        Debug.Log("[PlayerHealthManager] Start() aufgerufen – currentHealth: " + currentHealth);
    }
    public static PlayerHealthManager Instance;

    void Awake()
    {
        Instance = this;
    }

    public void TakeDamage(int damage)
    {
        int damageToApply = damage;
        Debug.Log("[PlayerHealthManager] TakeDamage() aufgerufen mit damage: " + damage);

        if (currentShield > 0)
        {
            if (damageToApply <= currentShield)
            {
                currentShield -= damageToApply;
                damageToApply = 0;
            }
            else
            {
                damageToApply -= currentShield;
                currentShield = 0;
            }

            UpdateShieldUI(); // Schildanzeige aktualisieren
        }


        currentHealth -= damageToApply;
        if (currentHealth < 0) currentHealth = 0;{
            UpdateHealthUI();

        }

        
        if (damagePopupSpawner != null)
        {
          //  DamagePopupSpawner.Instance.SpawnHeroDamagePopup(damage);

        }
        else
        {
            Debug.LogWarning("[PlayerHealthManager] damagePopupSpawner ist NULL!");
        }

        if (currentHealth <= 0)
        {
            Debug.Log("[PlayerHealthManager] currentHealth <= 0, GameOver wird ausgelöst.");
            gameOverManager.ShowGameOver();
        }
    }

    public void Heal(int amount)
    {
        Debug.Log("[PlayerHealthManager] Heal() aufgerufen mit amount: " + amount);

        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        UpdateHealthUI();

        // ➡️ Popup auslösen (in grün für Heilung)
        if (damagePopupSpawner != null)
        {
         //   DamagePopupSpawner.Instance.SpawnHeroHealPopup(amount);


        }
        else
        {
            Debug.LogWarning("[PlayerHealthManager] damagePopupSpawner ist NULL!");
        }
    }

    public void UpdateHealthUI()
    {
        Debug.Log("[PlayerHealthManager] UpdateHealthUI() aufgerufen – currentHealth: " + currentHealth);

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        if (healthText != null)
        {
            healthText.text = $"{currentHealth} / {maxHealth}";
        }
    }
    public void UpdateShieldUI()
    {
        if (shieldText != null)
        {
            shieldText.text = currentShield > 0 ? $" +{currentShield}" : "";
        }
    }




}
