using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public string mainMenuSceneName = "Main Menu";

    private void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false); 
    }

    public void ShowGameOver()
    {

        Currency currency = FindFirstObjectByType<Currency>();
        if (currency != null)
        {
            currency.ResetGold();
        }

        Time.timeScale = 0f; 
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
