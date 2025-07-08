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
    [Header("Story Mode Wellen")]
    
    [Header("UI")]
    public TextMeshProUGUI waveCounterText; 

    [SerializeField]
    public List<Enemy> activeEnemies = new List<Enemy>();

    public int currentWave = 1; 
    public int maxWaves = 10;   

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
    public void ReindexEnemies()
    {
        for (int i = 0; i < activeEnemies.Count; i++)
        {
            if (activeEnemies[i] != null)
            {
                activeEnemies[i].enemyIndex = i;
            }
        }
    }
    [System.Serializable]
    public class WaveEnemyData
    {
        public GameObject enemyPrefab;
        public Transform spawnPoint; // ➕ individueller Spawnpunkt
        public int customHealth = 100;
        public int customDamage = 10;
        public bool isBoss = false; // 🆕 Boss-Markierung
    }

    [System.Serializable]
    public class WaveData
    {
        public List<WaveEnemyData> enemies;
    }

    [System.Serializable]
    public class StoryWaveData
    {
        public List<WaveEnemyData> enemies;
    }

    public List<StoryWaveData> storyWaves = new List<StoryWaveData>();


    public void SpawnEnemies()
    {
        activeEnemies.Clear();

        if (currentWave <= storyWaves.Count)
        {
            Debug.Log($"Welle {currentWave} wird aus storyWaves geladen.");

            StoryWaveData currentWaveData = storyWaves[currentWave - 1]; // Index-Anpassung

            for (int i = 0; i < currentWaveData.enemies.Count; i++)
            {
                WaveEnemyData enemyData = currentWaveData.enemies[i];
                Transform spawnPoint = enemyData.spawnPoint;

                if (enemyData.enemyPrefab != null && spawnPoint != null)
                {
                    // ➕ Temporär Instanz zur Y-Offset-Abfrage vorbereiten
                    Enemy tempScript = enemyData.enemyPrefab.GetComponent<Enemy>();
                    float yOffset = tempScript != null ? tempScript.spawnYOffset : 0f;

                    // ✅ Korrigierte Position mit Y-Offset
                    Vector3 spawnPos = spawnPoint.position + new Vector3(0f, yOffset, 0f);
                    GameObject instance = Instantiate(enemyData.enemyPrefab, spawnPos, Quaternion.identity);

                    Enemy enemyScript = instance.GetComponent<Enemy>();

                    if (enemyScript != null)
                    {
                        // ➕ Werte aus WaveData übernehmen
                        enemyScript.maxHealth = enemyData.customHealth;
                        enemyScript.currentHealth = enemyData.customHealth;
                        enemyScript.attackDamage = enemyData.customDamage;
                        enemyScript.enemyIndex = enemyData.isBoss ? 4 : i;

                        // ➕ Zur aktiven Liste hinzufügen
                        activeEnemies.Add(enemyScript);
                    }
                }
                else
                {
                    Debug.LogWarning($"❌ Gegner oder Spawnpunkt fehlt in Wave {currentWave} an Index {i}!");
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
