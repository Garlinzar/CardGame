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

            // CardDisplay: optisches Setzen
            CardDisplay display = newCard.GetComponentInChildren<CardDisplay>();
            if (display != null)
            {
                if (display.cardImage != null)
                    display.cardImage.sprite = card.cardImage;

                if (display.manaText != null)
                    display.manaText.text = card.manaCost.ToString();
            }

            // CardDataHolder: zum späteren Spielen
            CardDataHolder holder = newCard.AddComponent<CardDataHolder>();
            holder.cardData = card;
        }
    }

    public void PlayCard()
    {
        if (CardSelector.selectedCard == null) return;

        CardDataHolder holder = CardSelector.selectedCard.GetComponent<CardDataHolder>();

        if (holder != null && gameManager != null)
        {
            int manaCost = holder.cardData.manaCost;

            if (gameManager.TrySpendMana(manaCost))
            {
                Destroy(CardSelector.selectedCard);
                CardSelector.selectedCard = null;
            }
            else
            {
                Debug.Log("Nicht genug Mana!");
            }
        }
    }
}
