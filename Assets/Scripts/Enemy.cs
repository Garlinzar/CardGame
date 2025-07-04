using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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

    [Header("Loot")]
    public int soulValue = 10; // Anzahl Seelen, die dieser Gegner beim Tod gibt
    public Vector2Int goldDropRange = new Vector2Int(5, 20); // Gold-Min-Max
    private Currency Currency;

    void Start()
    {
        Currency = FindFirstObjectByType<Currency>();
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0) return;

        // Animation beim Treffen
        EnemyPunchEffect punch = GetComponent<EnemyPunchEffect>();
        if (punch != null) punch.PlayPunch();

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        // Schaden anzeigen
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


        if (Currency != null)
        {
            Currency.AddSoul(soulValue);
            Debug.Log($"Du hast{soulValue} Seelen absorbiert!");

            float dropChance = Random.value; 

            if (dropChance <= 0.7f)
            {
                int goldAmount = Random.Range(goldDropRange.x, goldDropRange.y + 1);
                Currency.AddGold(goldAmount);
                Debug.Log($"Gegner hat {goldAmount} Gold gedroppt!");
            }
            else
            {
                Debug.Log("Kein Gold gedroppt (Chance verfehlt)");
            }
        }


        EnemySpawner.Instance.activeEnemies.Remove(this);
        Destroy(gameObject);
        EnemySpawner.Instance.ReindexEnemies(); // <--- Neu
   
    }


    public IEnumerator EnemiesAttackOneAfterAnother()
    {
        foreach (Enemy enemy in EnemySpawner.Instance.activeEnemies)
        {
            if (enemy != null && enemy.currentHealth > 0)
            {
                // Gegner-Angriffsanimation
                EnemyAttackEffect attack = enemy.GetComponent<EnemyAttackEffect>();
                if (attack != null)
                {
                    attack.PlayAttack();
                }

                // Spieler schädigen und Hit-Effekt auslösen
                var player = PlayerHealthManager.Instance;
                if (player != null)
                {
                    player.TakeDamage(enemy.attackDamage);

                    // Hole den Hit-Effekt
                    if (player.hitEffect != null)
                    {
                        player.hitEffect.PlayHit();
                    }

                    else
                    {
                        Debug.LogWarning("⚠️ PlayerHitEffect nicht gefunden!");
                    }
                }

                yield return new WaitForSeconds(0.6f); // Abstand
            }
        }
    }



    public void AttackPlayer(PlayerHealthManager player)
    {
        // Nur ein Gegner (z. B. Index 0 oder Boss) startet die Coroutine
        if (enemyIndex == 0 || enemyIndex == 4)
        {
            StartCoroutine(EnemiesAttackOneAfterAnother());
        }
    }
}
