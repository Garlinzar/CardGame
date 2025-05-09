using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField]
    public List<Enemy> activeEnemies = new List<Enemy>();

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

        foreach (EnemySpawnSlot slot in spawnSlots)
        {
            if (slot.enabled && slot.spawnPoint != null && slot.enemyPrefab != null)
            {
                GameObject instance = Instantiate(slot.enemyPrefab, slot.spawnPoint.position, Quaternion.identity);
                Enemy enemyScript = instance.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    activeEnemies.Add(enemyScript);
                }
            }
        }
    }
}
