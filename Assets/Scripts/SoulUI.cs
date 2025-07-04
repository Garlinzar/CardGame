using TMPro;
using UnityEngine;

public class SoulUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI soulText;

    private void Start()
    {
        SoulCurrency.Instance.SoulsChanged += UpdateUI;
        UpdateUI(SoulCurrency.Instance.CurrentSouls);
    }

    private void UpdateUI(int newAmount)
    {
        soulText.text = $"Seelen: {newAmount}";
    }

    private void OnDestroy()
    {
        if (SoulCurrency.Instance != null)
            SoulCurrency.Instance.SoulsChanged -= UpdateUI;
    }
}