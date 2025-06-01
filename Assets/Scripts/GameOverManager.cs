using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public string mainMenuSceneName = "Main Menu";

    private void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false); // Panel am Anfang deaktivieren
    }

    public void ShowGameOver()
    {
        Time.timeScale = 0f; // Spiel pausieren (optional)
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f; // Zeit zurücksetzen
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
