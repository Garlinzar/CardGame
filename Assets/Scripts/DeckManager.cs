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
        foreach (Transform child in cardHolder)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < amount && currentDeck.Count > 0; i++)
        {
            CardData card = currentDeck[0];
            currentDeck.RemoveAt(0);

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

    public void PlayCard()
    {
        if (CardSelector.selectedCard == null) return;

        CardDataHolder holder = CardSelector.selectedCard.GetComponent<CardDataHolder>();
        if (holder == null || holder.cardData == null) return;

        int manaCost = holder.cardData.manaCost;
        int damage = holder.cardData.damage;

        if (!gameManager.TrySpendMana(manaCost)) return;

        // Gegner über Singleton finden
        EnemySpawner spawner = EnemySpawner.Instance;
        if (spawner == null || spawner.activeEnemies == null) return;

        foreach (Enemy enemy in spawner.activeEnemies)
        {
            if (enemy != null && enemy.currentHealth > 0)
            {
                enemy.TakeDamage(damage);
                break;
            }
        }

        Destroy(CardSelector.selectedCard);
        CardSelector.selectedCard = null;
    }
}
