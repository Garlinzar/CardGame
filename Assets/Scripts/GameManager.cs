using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Mana")]
    public TextMeshProUGUI manaText;
    public int maxMana = 10;
    public int currentMana = 10;
    public Slider manaSlider;

    [Header("Referenzen")]
    public DeckManager deckManager; // damit wir deckManager.ResetMana() nicht brauchen
    public PlayerHealthManager playerHealthManager; // Referenz auf Spielerleben

    void Start()
    {
        UpdateManaUI();
    }

    public bool TrySpendMana(int amount)
    {
        if (currentMana >= amount)
        {
            currentMana -= amount;
            UpdateManaUI();
            return true;
        }
        return false;
    }

    public void UpdateManaUI()
    {
        if (manaSlider != null)
        {
            manaSlider.minValue = -1;
            manaSlider.maxValue = maxMana;
            manaSlider.value = currentMana;
        }

        if (manaText != null)
        {
            manaText.text = currentMana + " / " + maxMana;
        }
    }

    public void ResetMana()
    {
        currentMana = maxMana;
        UpdateManaUI();
    }

    // 🟡 Neue Methode für den End Turn Button
    public void OnEndTurn()
    {
        Debug.Log("Zug beendet. Mana wird zurückgesetzt und Gegner greifen an.");

        ResetMana(); // Mana voll machen

        if (EnemySpawner.Instance != null && EnemySpawner.Instance.activeEnemies != null)
        {
            foreach (Enemy enemy in EnemySpawner.Instance.activeEnemies)
            {
                if (enemy != null && enemy.currentHealth > 0)
                {
                    enemy.AttackPlayer(playerHealthManager); // Gegner greift an
                }
            }
        }
        else
        {
            Debug.LogWarning("Keine Gegner gefunden oder EnemySpawner.Instance ist null.");
        }
    }
}
