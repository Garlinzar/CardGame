using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Linq;

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
    [SerializeField] private GameObject heroObject;


    [Header("Runtime")]
    public List<CardData> currentDeck = new List<CardData>();

    [Header("UI")]
    public GameObject cardPrefab;
    public Transform cardHolder;

    public GameManager gameManager;

    [Header("Draw Settings")]
    public int drawCardManaCost = 1;
    public AudioClip cardPlaceSound; // Der einheitliche Sound fürs Kartenlegen
    public AudioSource audioSource;

    public bool doubleAttackNextDamageCard = false;


    void Start()
    {
       
            if (heroObject == null)
            {
                GameObject found = GameObject.Find("Hero");
                if (found != null)
                {
                    Debug.LogWarning("⚠️ heroObject wurde nicht gesetzt – automatisch gesetzt auf: " + found.name);
                    heroObject = found;
                }
                else
                {
                    Debug.LogError("❌ Hero konnte nicht automatisch gefunden werden!");
                }
            }

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

        if (holder.cardData.isDoubleNextAttackCard)
        {
            doubleAttackNextDamageCard = true;
            Debug.Log("🔁 Doppelschlag-Effekt aktiviert für nächste Angriffskarte!");
            Destroy(CardSelector.selectedCard);
            CardSelector.selectedCard = null;
            return; // Keine weiteren Effekte bei dieser Karte
        }

        int manaCost = holder.cardData.manaCost;
        int damage = holder.cardData.damage;
        int healAmount = holder.cardData.healPercent;
        int bonusManaNextTurn = holder.cardData.bonusManaNextTurn;

        if (!gameManager.TrySpendMana(manaCost)) return;

        StartCoroutine(PlayCardWithSounds(holder.cardData, damage, healAmount, bonusManaNextTurn));

        Destroy(CardSelector.selectedCard);
        CardSelector.selectedCard = null;
    }


    private IEnumerator PlayCardWithSounds(CardData cardData, int damage, int healAmount, int bonusManaNextTurn)
    {
        // Zuerst globaler Kartenlegen-Sound
        if (cardPlaceSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(cardPlaceSound);
            yield return new WaitForSeconds(cardPlaceSound.length);
        }

        // Danach optional Kartensound der Karte
        if (cardData.playSound != null)
        {
            AudioSource.PlayClipAtPoint(cardData.playSound, Camera.main.transform.position);
        }

        // Hero-Animation
        if (heroObject != null)
        {
            Debug.Log(" heroObject ist NICHT null – prüfen Damage: " + damage);
            PunchMoveSimple punch = heroObject.GetComponent<PunchMoveSimple>();
            if (punch != null && damage > 0)
            {
                Debug.Log(" PunchMoveSimple gefunden – führe Punch aus!");
                punch.DoPunch();
            }
            else
            {
                Debug.LogWarning(" PunchMoveSimple nicht gefunden am Hero!");
            }
        }
        else
        {
            Debug.LogWarning(" heroObject ist NULL!");
        }

        // Angriff durchführen (nur wenn damage > 0)
        int numberOfAttacks = 1;
        if (damage > 0 && doubleAttackNextDamageCard)
        {
            numberOfAttacks = 2;
            doubleAttackNextDamageCard = false;
        }

        if (damage > 0)
        {
            EnemySpawner spawner = EnemySpawner.Instance;
            if (spawner == null || spawner.activeEnemies == null) yield break;

            for (int i = 0; i < numberOfAttacks; i++)
            {
                // Optional Sound beim zweiten Angriff
                if (i > 0 && cardData.playSound != null)
                {
                    AudioSource.PlayClipAtPoint(cardData.playSound, Camera.main.transform.position);
                }

                // Spezialeffekt: Schaden auf 2 zufällige Gegner aufteilen
                if (cardData.splitDamageOnTwoEnemies)
                {
                    List<Enemy> aliveEnemies = new List<Enemy>();
                    foreach (Enemy e in spawner.activeEnemies)
                    {
                        if (e != null && e.currentHealth > 0)
                        {
                            aliveEnemies.Add(e);
                        }
                    }

                    if (aliveEnemies.Count >= 2)
                    {
                        List<Enemy> targets = aliveEnemies.OrderBy(x => Random.value).Take(2).ToList();

                        foreach (Enemy target in targets)
                        {
                            int actualDamage = damage;

                            if (cardData.doubleDamageIfFullHealth && target.currentHealth == target.maxHealth)
                            {
                                actualDamage *= 2;
                                Debug.Log("🎯 Doppelschaden aktiviert – Gegner hat volles Leben!");
                            }

                            target.TakeDamage(actualDamage);
                        }
                    }
                    else if (aliveEnemies.Count == 1)
                    {
                        Enemy target = aliveEnemies[0];
                        int actualDamage = damage;

                        if (cardData.doubleDamageIfFullHealth && target.currentHealth == target.maxHealth)
                        {
                            actualDamage *= 2;
                            Debug.Log("🎯 Doppelschaden aktiviert – Gegner hat volles Leben!");
                        }

                        target.TakeDamage(actualDamage);
                    }
                }
                else
                {
                    // Standardfall: erster lebender Gegner wird angegriffen
                    foreach (Enemy enemy in spawner.activeEnemies)
                    {
                        if (enemy != null && enemy.currentHealth > 0)
                        {
                            int actualDamage = damage;

                            if (cardData.doubleDamageIfFullHealth && enemy.currentHealth == enemy.maxHealth)
                            {
                                actualDamage *= 2;
                                Debug.Log("🎯 Doppelschaden aktiviert – Gegner hat volles Leben!");
                            }

                            enemy.TakeDamage(actualDamage);
                            break;
                        }
                    }
                }

                // Optional Delay zwischen den zwei Angriffen
                yield return new WaitForSeconds(0.5f);
            }
        }

        // Spieler heilen
        if (healAmount > 0)
        {
            if (gameManager.playerHealthManager != null)
            {
                int healingAmount = Mathf.RoundToInt(gameManager.playerHealthManager.maxHealth * (cardData.healPercent / 100f));
                gameManager.playerHealthManager.Heal(healingAmount);
            }
        }

        // Bonusmana für nächste Runde speichern
        if (bonusManaNextTurn > 0)
        {
            gameManager.AddBonusMana(bonusManaNextTurn);
        }
    }

}
