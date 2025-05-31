using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class CardEntry
{
    public CardData cardData;
    public int count;
}

public class DeckManager : MonoBehaviour
{
    [Header("Deck Setup")]
    public List<CardEntry> starterDeckEntries;

    [Header("Runtime")]
    public List<CardData> currentDeck = new List<CardData>();

    [Header("UI")]
    public GameObject cardPrefab;
    public Transform cardHolder;

    public GameManager gameManager;

    [Header("Draw Settings")]
    public int drawCardManaCost = 1;


    void Start()
    {
        BuildDeck(); 
    }

    public void BuildDeck()
    {
        currentDeck.Clear();

        foreach (CardEntry entry in starterDeckEntries)
        {
            for (int i = 0; i < entry.count; i++)
            {
                currentDeck.Add(entry.cardData);
            }
        }

        ShuffleDeck();
        DrawStartingHand(4);
    }

    public void ShuffleDeck()
    {
        for (int i = 0; i < currentDeck.Count; i++)
        {
            CardData temp = currentDeck[i];
            int rand = Random.Range(i, currentDeck.Count);
            currentDeck[i] = currentDeck[rand];
            currentDeck[rand] = temp;
        }
    }

    public void DrawCards(int amount)
    {
        if (!gameManager.TrySpendMana(drawCardManaCost))
        {
            Debug.Log("Nicht genug Mana zum Karten ziehen.");
            return;
        }

        foreach (Transform child in cardHolder)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < amount && currentDeck.Count > 0; i++)
        {
            if (currentDeck.Count == 0)
                break;

            CardData card = currentDeck[Random.Range(0, currentDeck.Count)];

            GameObject newCard = Instantiate(cardPrefab, cardHolder);

            CardDisplay display = newCard.GetComponentInChildren<CardDisplay>();
            if (display != null)
            {
                if (display.cardImage != null)
                    display.cardImage.sprite = card.cardImage;

                if (display.manaText != null)
                    display.manaText.text = card.manaCost.ToString();
            }

            CardDataHolder holder = newCard.AddComponent<CardDataHolder>();
            holder.cardData = card;
        }
    }

    public void DrawStartingHand(int amount)
    {

        if (currentDeck.Count == 0)
        {
            return;
        }

        // Alte Karten entfernen
        foreach (Transform child in cardHolder)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < amount; i++)
        {
            CardData card = currentDeck[Random.Range(0, currentDeck.Count)];

            GameObject newCard = Instantiate(cardPrefab, cardHolder);
            if (newCard == null)
            {
                Debug.LogError($"[DrawStartingHand] Fehler beim Instanziieren der Karte {card.name}");
                continue;
            }

            // Anzeige aktualisieren
            CardDisplay display = newCard.GetComponentInChildren<CardDisplay>();
            if (display != null)
            {
                if (display.cardImage != null)
                    display.cardImage.sprite = card.cardImage;

                if (display.manaText != null)
                    display.manaText.text = card.manaCost.ToString();
            }
            else
            {
                Debug.LogWarning("[DrawStartingHand] Kein CardDisplay an der neuen Karte gefunden.");
            }

            // Daten setzen
            CardDataHolder holder = newCard.AddComponent<CardDataHolder>();
            holder.cardData = card;

        }
    }


    public void PlayCard()
    {
        if (CardSelector.selectedCard == null) return;

        CardDataHolder holder = CardSelector.selectedCard.GetComponent<CardDataHolder>();
        if (holder == null || holder.cardData == null) return;

        int manaCost = holder.cardData.manaCost;
        int damage = holder.cardData.damage;
        int healAmount = holder.cardData.healAmount;
        int bonusManaNextTurn = holder.cardData.bonusManaNextTurn;

        if (!gameManager.TrySpendMana(manaCost)) return;

        // Schaden an Gegner zufügen
        EnemySpawner spawner = EnemySpawner.Instance;
        if (spawner != null && spawner.activeEnemies != null)
        {
            foreach (Enemy enemy in spawner.activeEnemies)
            {
                if (enemy != null && enemy.currentHealth > 0)
                {
                    enemy.TakeDamage(damage);
                    break;
                }
            }
        }

        // Spieler heilen
        if (healAmount > 0)
        {
            if (gameManager.playerHealthManager != null)
            {
                gameManager.playerHealthManager.Heal(healAmount);
            }
        }

        // Bonusmana für nächste Runde speichern
        if (bonusManaNextTurn > 0)
        {
            gameManager.AddBonusMana(bonusManaNextTurn);
        }

        Destroy(CardSelector.selectedCard);
        CardSelector.selectedCard = null;
    }
}
