using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI manaText;
    public int maxMana = 10;
    public int currentMana = 10;

    public Slider manaSlider; // Referenz zur UI-Anzeige

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
            // MinValue auf -1 gesetzt, damit 1/8 Mana optisch nicht komplett leer wirkt
            manaSlider.minValue = -1;
            manaSlider.maxValue = maxMana; 
            manaSlider.value = currentMana; // ← Verhältnis setzen
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
}
