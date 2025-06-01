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
    private int bonusMana = 0;
    private int pendingBonusMana = 0;
    public TextMeshProUGUI bonusManaText;


    [Header("Referenzen")]
    public DeckManager deckManager; // damit wir deckManager.ResetMana() nicht brauchen
    public PlayerHealthManager playerHealthManager; // Referenz auf Spielerleben

    void Start()
    {
        UpdateManaUI();
    }

    public bool TrySpendMana(int amount)
    {
        int bonusUsed = 0;

        // Zuerst BonusMana abziehen
        if (bonusMana > 0)
        {
            bonusUsed = Mathf.Min(bonusMana, amount);
            bonusMana -= bonusUsed;
            amount -= bonusUsed;
        }

        // Dann Normales Mana abziehen
        if (currentMana >= amount)
        {
            currentMana -= amount;
            UpdateManaUI();
            UpdateBonusManaUI();
            return true;
        }

        // Falls zu wenig Mana da ist: Bonus zurückgeben
        bonusMana += bonusUsed;
        UpdateBonusManaUI();
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
    public void AddBonusMana(int amount)
    {
        pendingBonusMana += amount;
        UpdateBonusManaUI();
    }
    public void ResetMana()
    {
        bonusMana = pendingBonusMana;
        pendingBonusMana = 0;

        currentMana = maxMana;
        UpdateManaUI();
        UpdateBonusManaUI();
    }



    // 🟡 Neue Methode für den End Turn Button
    public void OnEndTurn()
    {
        Debug.Log("Zug beendet. Mana wird zurückgesetzt und Gegner greifen an.");

        ResetMana();

        if (EnemySpawner.Instance != null && EnemySpawner.Instance.activeEnemies != null)
        {
            foreach (Enemy enemy in EnemySpawner.Instance.activeEnemies)
            {
                if (enemy != null && enemy.currentHealth > 0)
                {
                    enemy.AttackPlayer(playerHealthManager);
                }
            }

            // Prüfen, ob alle tot sind, danach neue Gegner spawnen
            if (EnemySpawner.Instance.AreAllEnemiesDead())
            {
                Debug.Log("Alle Gegner wurden besiegt! Neue Gegner spawnen.");
                EnemySpawner.Instance.SpawnEnemies();
            }
        }
        else
        {
            Debug.LogWarning("Keine Gegner gefunden oder EnemySpawner.Instance ist null.");
        }
    }
    public void UpdateBonusManaUI()
    {
        if (bonusManaText != null)
        {
            if (pendingBonusMana > 0)
            {
                bonusManaText.text = "+" + pendingBonusMana;
            }
            else if (bonusMana > 0)
            {
                bonusManaText.text = "+" + bonusMana;
            }
            else
            {
                bonusManaText.text = "";
            }
        }
    }


}
