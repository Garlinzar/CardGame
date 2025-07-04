using UnityEngine;

public class SoulCurrency : MonoBehaviour
{
    public static SoulCurrency Instance { get; private set; }

    [SerializeField]
    private int currentSouls = 0;

    public int CurrentSouls => currentSouls;

    public delegate void OnSoulsChanged(int newAmount);
    public event OnSoulsChanged SoulsChanged;

    private void Awake()
    {
        // Singleton-Pattern für einfachen Zugriff
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Optional, falls persistierend
    }

 
    /// Erhöht die Anzahl der Seelen z. B. nach einem Kill.

    public void AddSouls(int amount)
    {
        currentSouls += amount;
        SoulsChanged?.Invoke(currentSouls); // UI kann darauf reagieren
    }


    /// Gibt Seelen aus, z. B. beim Kaufen.

    public bool SpendSouls(int amount)
    {
        if (currentSouls >= amount)
        {
            currentSouls -= amount;
            SoulsChanged?.Invoke(currentSouls);
            return true;
        }

        return false;
    }
}
