using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthManager : MonoBehaviour
{
    public Slider healthSlider;
    public TextMeshProUGUI healthText;

    public int maxHealth = 20;
    public int currentHealth;

    public GameOverManager gameOverManager;

    [Header("Damage Popup")]
    public DamagePopupSpawner damagePopupSpawner;  // Referenz zum Spawner-Objekt

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
        Debug.Log("[PlayerHealthManager] Start() aufgerufen – currentHealth: " + currentHealth);
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("[PlayerHealthManager] TakeDamage() aufgerufen mit damage: " + damage);

        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        UpdateHealthUI();

        // ➡️ Popup auslösen (in rot für Schaden)
        if (damagePopupSpawner != null)
        {
            Vector3 popupPosition = transform.position + Vector3.up * 2.0f;
            Debug.Log("[PlayerHealthManager] Schaden-Popup wird gespawnt an Position: " + popupPosition);
            damagePopupSpawner.SpawnPlayerDamagePopup(-damage, Color.red);
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
            Vector3 popupPosition = transform.position + Vector3.up * 2.0f;
            Debug.Log("[PlayerHealthManager] Heil-Popup wird gespawnt an Position: " + popupPosition);
            damagePopupSpawner.SpawnPlayerDamagePopup(+amount, Color.green);
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
}
