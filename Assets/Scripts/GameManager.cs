using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections; 

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
    public DeckManager deckManager;
    public PlayerHealthManager playerHealthManager;
    public Button endTurnButton;


    [Header("Enemy Attack Settings")]
    public float enemyAttackDelay = 0.5f; // Delay in Sekunden zwischen den Attacken

    private bool canEndTurn = true;
    public float endTurnCooldown = 1f;


    void Start()
    {
        UpdateManaUI();
    }

    public bool TrySpendMana(int amount)
    {
        int bonusUsed = 0;

        if (bonusMana > 0)
        {
            bonusUsed = Mathf.Min(bonusMana, amount);
            bonusMana -= bonusUsed;
            amount -= bonusUsed;
        }

        if (currentMana >= amount)
        {
            currentMana -= amount;
            UpdateManaUI();
            UpdateBonusManaUI();
            return true;
        }

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

    public void OnEndTurn()
    {
        if (!canEndTurn)
        {
            Debug.Log("End Turn ist noch im Cooldown!");
            return;
        }

        canEndTurn = false;
        if (endTurnButton != null)
        {
            endTurnButton.interactable = false;
        }

        Debug.Log("Zug beendet. Mana wird zurückgesetzt und Gegner greifen an.");

        ResetMana();

        if (EnemySpawner.Instance != null && EnemySpawner.Instance.activeEnemies != null)
        {
            StartCoroutine(EnemyAttackSequence());
        }
        else
        {
            Debug.LogWarning("Keine Gegner gefunden oder EnemySpawner.Instance ist null.");
        }

        StartCoroutine(ResetEndTurnCooldown());
    }
    private IEnumerator ResetEndTurnCooldown()
    {
        yield return new WaitForSeconds(endTurnCooldown);
        canEndTurn = true;
        if (endTurnButton != null)
        {
            endTurnButton.interactable = true;
        }
    }


    private IEnumerator EnemyAttackSequence()
    {
        foreach (Enemy enemy in EnemySpawner.Instance.activeEnemies)
        {
            if (enemy != null && enemy.currentHealth > 0)
            {
                enemy.AttackPlayer(playerHealthManager);
                yield return new WaitForSeconds(enemyAttackDelay);
            }
        }

        if (EnemySpawner.Instance.AreAllEnemiesDead())
        {
            Debug.Log("Alle Gegner wurden besiegt! Neue Gegner spawnen.");
            EnemySpawner.Instance.SpawnEnemies();
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
