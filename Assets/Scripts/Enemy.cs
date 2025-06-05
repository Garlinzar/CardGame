using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 10;
    public int currentHealth = 10;
    public int attackDamage = 2;
    public int enemyIndex;
    public Transform popupSpawnPoint;

    [Header("UI")]
    public Slider healthSlider;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0)
        {
            // Kein Schaden = kein Popup spawnen
            return;
        }

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        // Damage-Popup-Logik
        if (DamagePopupSpawner.Instance != null)
        {
            DamagePopupSpawner.Instance.SpawnEnemyDamagePopup(enemyIndex, -damage, Color.red);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }


    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = (float)currentHealth / maxHealth;
        }
    }

    private void Die()
    {
        // Hier kannst du später Animation, FX oder Destroy hinzufügen
        Destroy(gameObject);
    }

    public void AttackPlayer(PlayerHealthManager player)
    {
        if (player != null)
        {
            player.TakeDamage(attackDamage);
        }
    }
}
