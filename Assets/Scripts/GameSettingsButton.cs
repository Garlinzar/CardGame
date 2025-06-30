using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSettingsButton : MonoBehaviour

{
    [Header("Ingame Menu Buttons")]
    [SerializeField] private Button SettingsButton;

    [Header("Scene Management")]
    [SerializeField] private string gameplaySceneName = "GameScene";

    private void Start()
    {
        // Set up ingame menu button listeners
        if (SettingsButton) SettingsButton.onClick.AddListener(OpenIngameMenu);

    }

    public void OpenIngameMenu()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }
}
    
