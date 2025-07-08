using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopupSpawner : MonoBehaviour
{
    public static DamagePopupSpawner Instance;

    [Header("Popups")]
    public GameObject damagePopupPrefab;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// Spawnt einen Schaden-Popup an einer konkreten Weltposition (z. B. über dem Gegner)
    /// </summary>
    public void SpawnEnemyDamagePopup(Transform enemyTransform, int amount, Color textColor)
    {
        if (damagePopupPrefab == null || enemyTransform == null)
        {
            Debug.LogWarning("❌ PopupPrefab oder Gegnertransform fehlt!");
            return;
        }

        // Spawne das Popup direkt als Kind des Gegners oder daneben
        GameObject popup = Instantiate(damagePopupPrefab, enemyTransform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
        popup.transform.SetParent(enemyTransform); // Optional: mitbewegen mit Gegner

        TextMeshProUGUI text = popup.GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
        {
            text.text = amount > 0 ? "+" + amount : amount.ToString();
            text.color = textColor;
        }

        Debug.Log($"📦 DamagePopup gespawnt über {enemyTransform.name} mit Schaden: {amount}");
    }

   
}
