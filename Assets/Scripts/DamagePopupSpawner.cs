using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopupSpawner : MonoBehaviour
{
    public static DamagePopupSpawner Instance;

    [Header("Popups")]
    public GameObject damagePopupPrefab;

    [Header("Spawnpunkte")]
    public Transform playerSpawnPoint;
    public List<Transform> enemySpawnPoints = new List<Transform>();
    public Transform bossPopupPoint; // Neu für den Boss (Index 4)

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// Spawnt einen Schaden-Popup für den Spieler
    /// </summary>
    public void SpawnPlayerDamagePopup(int amount, Color textColor)
    {
        if (damagePopupPrefab == null || playerSpawnPoint == null)
        {
            Debug.LogWarning("DamagePopupPrefab oder PlayerSpawnPoint fehlt!");
            return;
        }

        SpawnDamagePopupAtPosition(playerSpawnPoint.position, amount, textColor);
    }

    /// <summary>
    /// Spawnt einen Schaden-Popup für einen Gegner
    /// </summary>
    public void SpawnEnemyDamagePopup(int enemyIndex, int amount, Color textColor)
    {
        if (damagePopupPrefab == null)
        {
            Debug.LogWarning("DamagePopupPrefab fehlt!");
            return;
        }

        // Prüfen, ob es sich um den Boss (Index 4) handelt:
        if (enemyIndex == 4)
        {
            if (bossPopupPoint != null)
            {
                SpawnDamagePopupAtPosition(bossPopupPoint.position, amount, textColor);
            }
            else
            {
                Debug.LogWarning("BossPopupPoint fehlt!");
            }
        }
        // Normale Gegner (Index 0–2)
        else if (enemyIndex >= 0 && enemyIndex < enemySpawnPoints.Count)
        {
            if (enemySpawnPoints[enemyIndex] != null)
            {
                SpawnDamagePopupAtPosition(enemySpawnPoints[enemyIndex].position, amount, textColor);
            }
            else
            {
                Debug.LogWarning($"EnemySpawnPoint {enemyIndex} fehlt!");
            }
        }
        else
        {
            Debug.LogWarning("Ungültiger enemyIndex oder kein enemySpawnPoint!");
        }
    }

    /// <summary>
    /// Gemeinsame Hilfsmethode
    /// </summary>
    private void SpawnDamagePopupAtPosition(Vector3 position, int amount, Color textColor)
    {
        GameObject popup = Instantiate(damagePopupPrefab, position, Quaternion.identity, GameObject.Find("Canvas").transform);
        TextMeshProUGUI text = popup.GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
        {
            text.text = amount > 0 ? "+" + amount : amount.ToString();
            text.color = textColor;
        }
    }
}
