using UnityEngine;
using System.IO;

public class SaveLoadManager : MonoBehaviour
{
    private string filePath;

    void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, "playerData.json");
    }

    public void SaveData(PlayerData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
        Debug.Log("Daten gespeichert unter: " + filePath);
    }

    public PlayerData LoadData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            return data;
        }
        else
        {
            Debug.Log("Keine gespeicherten Daten gefunden, neue Daten werden erstellt.");
            return new PlayerData(); // Standardwerte
        }
    }

    public void DeleteData()
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("Spielstand gelöscht");
        }
    }
}
