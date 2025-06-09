using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }

    [System.Serializable]
    public class EnemySpawnSlot
    {
        public Transform spawnPoint;
        public GameObject enemyPrefab;
        public bool enabled = true;
    }

    [Header("Spawn-Slots (1–3 Gegner individuell)")]
    public EnemySpawnSlot[] spawnSlots;

    [Header("Boss")]
    public GameObject bossPrefab;
    public Transform bossSpawnPoint;  // Mittlerer Slot

    [Header("UI")]
    public TextMeshProUGUI waveCounterText; // 🔥 Hier für die Anzeige

    [SerializeField]
    public List<Enemy> activeEnemies = new List<Enemy>();

    public int currentWave = 1; // Neu: Zähler für die aktuelle Welle
    public int maxWaves = 10;   // Neu: Maximale Wellenzahl (inkl. Boss)

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        SpawnEnemies();
    }

    public void SpawnEnemies()
    {
        activeEnemies.Clear();

        if (currentWave < maxWaves)
        {
            Debug.Log($"Welle {currentWave} startet: Normale Gegner spawnen.");

            for (int i = 0; i < spawnSlots.Length; i++)
            {
                EnemySpawnSlot slot = spawnSlots[i];
                if (slot.enabled && slot.spawnPoint != null && slot.enemyPrefab != null)
                {
                    GameObject instance = Instantiate(slot.enemyPrefab, slot.spawnPoint.position, Quaternion.identity);
                    Enemy enemyScript = instance.GetComponent<Enemy>();
                    if (enemyScript != null)
                    {
                        enemyScript.enemyIndex = i; // Index 1–3 für Damage Popups
                        activeEnemies.Add(enemyScript);
                    }
                }
            }
        }
        else if (currentWave == maxWaves)
        {
            Debug.Log("Welle 10 startet: BOSS spawnt!");
            if (bossPrefab != null && bossSpawnPoint != null)
            {
                GameObject bossInstance = Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);
                Enemy bossScript = bossInstance.GetComponent<Enemy>();
                if (bossScript != null)
                {
                    bossScript.enemyIndex = 4;  // Index 4 für Popups
                    activeEnemies.Add(bossScript);
                }
            }
        }
        else
        {
            Debug.Log("Alle Wellen abgeschlossen!");
        }
        UpdateWaveCounterUI();

        currentWave++;
    }

    public bool AreAllEnemiesDead()
    {
        if (activeEnemies == null || activeEnemies.Count == 0)
            return true;

        foreach (Enemy enemy in activeEnemies)
        {
            if (enemy != null && enemy.currentHealth > 0)
                return false;
        }
        return true;
    }
    private void UpdateWaveCounterUI()
    {
        if (waveCounterText != null)
        {
            waveCounterText.text = $"Wave {currentWave}";
        }
    }

}
