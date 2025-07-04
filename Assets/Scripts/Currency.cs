using UnityEngine;
using TMPro;

public class Currency : MonoBehaviour
{
    [Header("Soul Currency")]
    [SerializeField] private int countSoul;
    [SerializeField] private TMP_Text soulText;

    [Header("Gold Currency")]
    [SerializeField] private int countGold;
    [SerializeField] private TMP_Text goldText;

    void Start()
    {
        // Lade gespeicherte Werte
        countSoul = PlayerPrefs.GetInt("AmountSoul", 0);
        countGold = PlayerPrefs.GetInt("AmountGold", 0);

        UpdateSoulText();
        UpdateGoldText();
    }

    // SOULS

    public void AddSoul(int amount)
    {
        countSoul += amount;
        PlayerPrefs.SetInt("AmountSoul", countSoul);
        UpdateSoulText();
    }

    public bool SpendSoul(int amount)
    {
        if (countSoul >= amount)
        {
            countSoul -= amount;
            PlayerPrefs.SetInt("AmountSoul", countSoul);
            UpdateSoulText();
            return true;
        }
        else
        {
            Debug.Log("Nicht genug Seelen!");
            return false;
        }
    }

    // GOLD

    public void AddGold(int amount)
    {
        countGold += amount;
        PlayerPrefs.SetInt("AmountGold", countGold);
        UpdateGoldText();
    }

    public bool SpendGold(int amount)
    {
        if (countGold >= amount)
        {
            countGold -= amount;
            PlayerPrefs.SetInt("AmountGold", countGold);
            UpdateGoldText();
            return true;
        }
        else
        {
            Debug.Log("Nicht genug Gold!");
            return false;
        }
    }

    public void ResetGold()
    {
        countGold = 0;
        PlayerPrefs.SetInt("AmountGold", countGold);
        UpdateGoldText();
    }

    // UI Updates

    private void UpdateSoulText()
    {
        if (soulText != null)
            soulText.text = countSoul.ToString();
    }

    private void UpdateGoldText()
    {
        if (goldText != null)
            goldText.text = countGold.ToString();
    }
}
